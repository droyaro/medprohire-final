(function ($) {

    "use strict";

    // REMOVE # FROM URL
    $('a[href="#"]').click(function (e) {
        e.preventDefault();
    });

    // COUNTER
    function count($this) {
        var current = parseInt($this.html(), 10);
        $this.html(++current);
        if (current !== $this.data('count')) {
            setTimeout(function () { count($this) }, 50);
        }
    }
    $(".badges-counter").each(function () {
        $(this).data('count', parseInt($(this).html(), 10));
        $(this).html('0');
        count($(this));
    });

    	// Select Picker
  
    // Tabs
    $(".home-plan-info-tabs a, .tabs-1 a").click(function (e) {
        e.preventDefault();
        $(this).tab('show');
    });

    $(".datepickerday").datepicker({
      
        endDate: 'M',
       
    });
    $(".datepickercert").datepicker({

        endDate: 'M',

    });
    $(".datepickerday-issue").datepicker({
        format: "mm-yyyy",
        startView: "months",
        minViewMode: "months",
      

    });
    //$(".datepicker").datepicker({
    //    dateFormat: 'dd/mm/yyyy' ,


    //});


    // ACCORDION
    var $active = $(".accordion-1 .collapse.show, .accordion-2 .collapse.show, .accordion-3 .collapse.show").prev().addClass("active");

    $active.find("a").append("<span class=\"fa fa-angle-up float-right\"></span>");

    $(".accordion-1 .card-heading, .accordion-2 .card-heading, .accordion-3 .card-heading").not($active).find('a').prepend("<span class=\"fa fa-angle-down float-right\"></span>");

    $(".accordion-1, .accordion-2, .accordion-3").on("shown.bs.collapse", function (e) {
        $(".accordion-1 .card-heading.active, .accordion-2 .card-heading.active, .accordion-3 .card-heading.active")
            .removeClass("active")
            .find(".fa")
            .toggleClass("fa-angle-down fa-angle-up");
        $(e.target)
            .prev()
            .addClass("active")
            .find(".fa")
            .toggleClass("fa-angle-down fa-angle-up");
    });

    $(".accordion-1, .accordion-2, .accordion-3").on("hidden.bs.collapse", function (e) {
        $(e.target)
            .prev()
            .removeClass("active")
            .find(".fa")
            .removeClass("fa-angle-up")
            .addClass("fa-angle-down");
    });

    //MAGNIFIC POPUP
    $(".home-gallery-item").magnificPopup({
        delegate: 'a.zoom',
        type: 'image',
        gallery: {
            enabled: true
        }
    });

    // Team  Carousel
    //$("#home-news-carousel").owlCarousel({
    //    autoPlay: true, //Set AutoPlay to 3 seconds
    //    items: 3,
    //    stopOnHover: true,
    //    navigation: true, // Show next and prev buttons
    //    pagination: false,
    //    navigationText: ["<span class='fa fa-angle-left'></span>", "<span class='fa fa-angle-right'></span>"]
    //});


    // Price Slider
    var $element = $('input[type="range"]');
    var $handle;

    $element.rangeslider({
        polyfill: false,
        disabledClass: 'rangeslider--disabled',
        onInit: function () {
            
          
            $handle = $('.rangeslider__handle', this.$range);
            updateHandle($handle[0], this.value);
            $("#amount-label-1, #amount-label-2, #amount-label-3, #amount-label-4").attr('style', 'margin-left:' +8+ 'px;')
            $("#amount-label-1, #amount-label-2, #amount-label-3, #amount-label-4").html(this.value + '<span class="pricing-dollar"></span>' );
        }
    }).on('input', function () {
        updateHandle($handle[0], this.value);
        
        var rangerfill = $('.rangeslider__fill').width() - 8;
        if (this.value > 100) { rangerfill = $('.rangeslider__fill').width() - 11; }
        if (this.value < 10) { rangerfill = $('.rangeslider__fill').width() - 5; }
        if (this.value ==0) { rangerfill = $('.rangeslider__fill').width() - 5; }
        $("#amount-label-1, #amount-label-2, #amount-label-3, #amount-label-4").attr('style', 'margin-left:' + rangerfill+'px;')
        $("#amount-label-1, #amount-label-2, #amount-label-3, #amount-label-4").html(this.value+ '<span class="pricing-dollar"></span>'  );
    });

    function updateHandle(el, val) {
        //el.textContent = val;
    }

    $('input[type="range"]').rangeslider(
    );

    // Select Picker
    //$(".selectpicker").selectpicker({
    //    style: 'btn-default'
    //});
    $(document).on('click', '#myBtn', function (e) {

        var height = 0;
        $(this).parent().find(".readmoretext").each(function (i) {
            height += $(this).prop('scrollHeight');
         
        });

        if ($(this).parent().find(".readmoretext").hasClass('opened')) {
            $(this).parent().find(".readmoretext").removeClass('opened').css('height', '80px');
           

            $(this).text('Read More');
        } else {
            $(this).parent().find(".readmoretext").addClass('opened').css('height', height + 'px');
           
            $(this).text('Read Less');
        }
        e.preventDefault();
    });
    //$(document).on('click', '#myBtn', function () {
    //    var dots = $(this).parent().find('#dots');
    //    var moreText = $(this).parent().find('#more');

    //    var btnText = $(this);

    //    if (btnText[0].innerHTML === "Read less") {
    //        dots[0].style.display = "inline";
    //        btnText[0].innerHTML = "Read more";
    //        $(this).parent().find('#more').fadeOut();
    //    } else {
    //        dots[0].style.display = "none";
    //        btnText[0].innerHTML = "Read less";
    //        $(this).parent().find('span').fadeIn();
           
           
    //    }
   
    //});
    //AJAX CONTACT FORM
    $(".contact-form").submit(function () {
        var rd = this;
        var url = "sendemail.php"; // the script where you handle the form input.
        $.ajax({
            type: "POST",
            url: url,
            data: $(".contact-form").serialize(), // serializes the form's elements.
            success: function (data) {
                //alert("Mail sent!"); // show response from the php script.
                $(rd).prev().text(data.message).fadeIn().delay(3000).fadeOut();
            }
        });
        return false; // avoid to execute the actual submit of the form.
    });
    function previewProfileImage(uploader) {
        //ensure a file was selected 
        if (uploader.files && uploader.files[0]) {
            var imageFile = uploader.files[0];
            var reader = new FileReader();
            reader.onload = function (e) {
                //set the image data as source
              
                $('.profile').attr('src', e.target.result);
                $('#uploadimg').parent().attr('hidden', false);
                $('#saveimage').parent().attr('hidden', false);
                $('.u-block-hover').addClass('hover');
                $('.cropper-canvas img').attr('src', e.target.result);
                $('.cropper-view-box img').attr('src', e.target.result);
               
            }
            reader.readAsDataURL(imageFile);
        }
    }

    $("#ProfileImage").change(function () {
        previewProfileImage(this);
    });
    $("#Logo").change(function () {
        previewProfileImage(this);
    });

})(window.jQuery);


//$(".notifications").ready(function () {
//    $.ajax({
//        url: "/Home/_Notifications",
//        type: "POST",
        
//        success: function (response) {
//            $(".notifications").append(response);
//        }

//    });
    
//    });
        
