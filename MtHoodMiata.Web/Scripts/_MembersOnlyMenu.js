window.onerror = null;

btn_logout_off = new Image(139, 24);
btn_logout_off.src = "/images/buttons/btn_logout.gif";
btn_logout_over = new Image(139, 24);
btn_logout_over.src = "/images/buttons/btn_logout_over.gif";

btn_memberonlyhome_off = new Image(139, 24);
btn_memberonlyhome_off.src = "/images/buttons/btn_memberonlyhome.gif";
btn_memberonlyhome_over = new Image(139, 24);
btn_memberonlyhome_over.src = "/images/buttons/btn_memberonlyhome_over.gif";

btn_editmembership_off = new Image(139, 24);
btn_editmembership_off.src = "/images/buttons/btn_editmembership.gif";
btn_editmembership_over = new Image(139, 24);
btn_editmembership_over.src = "/images/buttons/btn_editmembership_over.gif";

btn_classifieds_off = new Image(139, 24);
btn_classifieds_off.src = "/images/buttons/btn_classifieds.gif";
btn_classifieds_over = new Image(139, 24);
btn_classifieds_over.src = "/images/buttons/btn_classifieds_over.gif";

btn_changepassword_off = new Image(139, 24);
btn_changepassword_off.src = "/images/buttons/btn_changepassword.gif";
btn_changepassword_over = new Image(139, 24);
btn_changepassword_over.src = "/images/buttons/btn_changepassword_over.gif";

btn_boardonly_off = new Image(139, 24);
btn_boardonly_off.src = "/images/buttons/btn_boardonly.gif";
btn_boardonly_over = new Image(139, 24);
btn_boardonly_over.src = "/images/buttons/btn_boardonly_over.gif";

btn_constitution_off = new Image(139, 24);
btn_constitution_off.src = "/images/buttons/btn_constitution.gif";
btn_constitution_over = new Image(139, 24);
btn_constitution_over.src = "/images/buttons/btn_constitution_over.gif";

btn_newsletter_off = new Image(139, 24);
btn_newsletter_off.src = "/images/buttons/btn_newsletter.gif";
btn_newsletter_over = new Image(139, 24);
btn_newsletter_over.src = "/images/buttons/btn_newsletter_over.gif";

btn_roster_off = new Image(139, 24);
btn_roster_off.src = "/images/buttons/btn_roster.gif";
btn_roster_over = new Image(139, 24);
btn_roster_over.src = "/images/buttons/btn_roster_over.gif";

btn_home_off = new Image(139, 24);
btn_home_off.src = "/images/buttons/btn_home.gif";
btn_home_over = new Image(139, 24);
btn_home_over.src = "/images/buttons/btn_home_over.gif";

btn_uploadphotos_off = new Image(139, 24);
btn_uploadphotos_off.src = "/images/buttons/btn_uploadphotos.gif";
btn_uploadphotos_over = new Image(139, 24);
btn_uploadphotos_over.src = "/images/buttons/btn_uploadphotos_over.gif";

function ActiveOverImage(imageName) {
    imgActive = eval(imageName + "_over.src");
    document[imageName].src = imgActive;
}

function InActiveImage(imageName) {
    imgInActive = eval(imageName + "_off.src");
    document[imageName].src = imgInActive;
}
