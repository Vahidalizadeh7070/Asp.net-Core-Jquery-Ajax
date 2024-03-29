﻿$('#stars li').on('mouseover', function () {
    var onStar = parseInt($(this).data('value'), 10);
    $(this).parent().children('li.star').each(function (e) {
        if (e < onStar) {
            $(this).addClass('hover');
        }
        else {
            $(this).removeClass('hover');
        }
    });

}).on('mouseout', function () {
    $(this).parent().children('li.star').each(function (e) {
        $(this).removeClass('hover');
    });
});


/* 2. Action to perform on click */
$('#stars li').on('click', function () {
    var onStar = parseInt($(this).data('value'), 10); // The star currently selected
    var stars = $(this).parent().children('li.star');

    for (i = 0; i < stars.length; i++) {
        $(stars[i]).removeClass('selected');
    }

    for (i = 0; i < onStar; i++) {
        $(stars[i]).addClass('selected');
    }

    // JUST RESPONSE (Not needed)
    var ratingValue = parseInt($('#stars li.selected').last().data('value'), 10);
    var msg = "";
    if (ratingValue > 1) {
        msg = "Thanks! You rated this application " + ratingValue + " stars.";
    }
    else {
        msg = "We will improve ourselves. You rated this " + ratingValue + " stars.";
    }
    Rate(msg, ratingValue);

});
  
  


