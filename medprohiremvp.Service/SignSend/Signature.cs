using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Identity;
using medprohiremvp.DATA.IdentityModels;
using System.Linq;
using medprohiremvp.DATA.Entity;
using medprohiremvp.Service.IServices;
using Microsoft.AspNetCore.Hosting;



namespace medprohiremvp.Service.SignSend
{

    public class Signature : ISignature
    {
        private readonly ICommonServices _commonService;
        private readonly IHostingEnvironment _environment;
        private const string integratorKey = "344733fa-f9f9-4c67-94f6-914df8f0e040";
        private const string username = "nare.sargsian@gmail.com";
        private const string password = "As123!";
        private const string accountId = "7014292";
        // Additional constants
        private const string signerClientId = "1000";
        private const string basePath = "https://demo.docusign.net/restapi";
        // Change the port number in the Properties / launchSettings.json file:
        //     "iisExpress": {
        //        "applicationUrl": "http://localhost:5050",


        public Signature(ICommonServices commonServices, IHostingEnvironment environment)
        {
            _commonService = commonServices;
            _environment = environment;
        }
        public string Geturlsignature(string filename, string filepath, string email, string name, Guid userId, string returnurl, int xposition, int yposition, int pagenamber, int? emp_xposition, int? emp_yposition, int? emp_pagenamber)
        {
            // Embedded Signing Ceremony
            // 1. Create envelope request obj
            // 2. Use the SDK to create and send the envelope
            // 3. Create Envelope Recipient View request obj
            // 4. Use the SDK to obtain a Recipient View URL
            // 5. Redirect the user's browser to the URL

            // 1. Create envelope request object
            //    Start with the different components of the request
            //    Create the document object
            Document document = new Document
            {
                DocumentBase64 = Convert.ToBase64String(ReadContent(filepath)),
                Name = filename,
                FileExtension = "pdf",
                DocumentId = "1"
            };
            Document[] documents = new Document[] { document };

            // Create the signer recipient object 
            Signer signer = new Signer
            {
                Email = email,
                Name = name,
                ClientUserId = signerClientId,
                RecipientId = "1",
                RoutingOrder = "1"
            };
            // Create the sign here tab (signing field on the document)
            SignHere signHereTab = new SignHere
            {
                DocumentId = "1",
                PageNumber = pagenamber.ToString(),
                RecipientId = "1",
                TabLabel = "Sign Here Tab",
                XPosition =xposition.ToString(),
                YPosition  =yposition.ToString()
            };
            SignHere[] signHereTabs = new SignHere[] { signHereTab };

            // Add the sign here tab array to the signer object.
            signer.Tabs = new Tabs { SignHereTabs = new List<SignHere>(signHereTabs) };
            // Create array of signer objects
            Signer[] signers = new Signer[] { signer };
            // Create recipients object
            Recipients recipients = new Recipients { Signers = new List<Signer>(signers) };
            // Bring the objects together in the 

            EnvelopeDefinition envelopeDefinition = new EnvelopeDefinition
            {
                EmailSubject = "Please sign the document",
                Documents = new List<Document>(documents),
                Recipients = recipients,
                Status = "sent"
            };

            // 2. Use the SDK to create and send the envelope
            ApiClient apiClient = new ApiClient(basePath);
            string authHeader = "{\"Username\":\"" + username + "\", \"Password\":\"" + password + "\", \"IntegratorKey\":\"" + integratorKey + "\"}";
            apiClient.Configuration.AddDefaultHeader("X-DocuSign-Authentication", authHeader);
            EnvelopesApi envelopesApi = new EnvelopesApi(apiClient.Configuration);
            EnvelopeSummary results = envelopesApi.CreateEnvelope(accountId, envelopeDefinition);
            // 3. Create Envelope Recipient View request obj
            string envelopeId = results.EnvelopeId;
            SignSent sign = new SignSent() { User_ID = userId, Envelope_ID = envelopeId, Status = "", FileType=filename, Emp_XPosition=emp_xposition.GetValueOrDefault(), Emp_YPosition=emp_yposition.GetValueOrDefault(), Emp_PageNumber=emp_pagenamber.GetValueOrDefault() };

            Guid signId = _commonService.InsertEnvelopepId(sign);

            if (signId != Guid.Empty)
            {
                returnurl += "?signId=" + signId;
            }
            RecipientViewRequest viewOptions = new RecipientViewRequest
            {

                ReturnUrl = returnurl,
                ClientUserId = signerClientId,
                AuthenticationMethod = "none",
                UserName = name,
                Email = email,
            };
            // 4. Use the SDK to obtain a Recipient View URL

            ViewUrl viewUrl = envelopesApi.CreateRecipientView(accountId, envelopeId, viewOptions);
            return viewUrl.Url;

        }
        internal static byte[] ReadContent(string filepath)
        {
            byte[] buff = null;
            string path = filepath;
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    long numBytes = new FileInfo(path).Length;
                    buff = br.ReadBytes((int)numBytes);
                }
            }
            return buff;
        }
        public string Downloadsignfile(string Envelope_id, string user_id, string filename)
        {
            string filePath = "";
            FileStream fs = null;
            string foldername = "Upload//" + filename;
            EnvelopesApi envelopesApi = new EnvelopesApi();
            EnvelopeDocumentsResult docsList = envelopesApi.ListDocuments(accountId, Envelope_id);
            string status=envelopesApi.GetEnvelope(accountId, Envelope_id).Status;
            string path = (Path.Combine(_environment.WebRootPath, foldername));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (status == "completed")
            {
                try
                {
                    for (int i = 0; i < docsList.EnvelopeDocuments.Count; i++)
                    {
                        if (docsList.EnvelopeDocuments[i].Name.ToLower() == filename.ToLower())
                        {
                            // GetDocument() API call returns a MemoryStream
                            MemoryStream docStream = (MemoryStream)envelopesApi.GetDocument(accountId, docsList.EnvelopeId, docsList.EnvelopeDocuments[i].DocumentId);
                            // let's save the document to local file system
                            filePath = path + "\\" + user_id + ".pdf";
                            fs = new FileStream(filePath, FileMode.Create);
                            docStream.Seek(0, SeekOrigin.Begin);
                            docStream.CopyTo(fs);
                            fs.Close();
                        }

                    }
                    return foldername + "\\" + user_id + ".pdf";
                }
                catch
                { return string.Empty; }
            }
            return string.Empty;
        }
    }
}
