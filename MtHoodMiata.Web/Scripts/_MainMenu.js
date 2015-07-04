window.onerror = null;

btn_membersonly_off = new Image(139, 24);
btn_membersonly_off.src = "/images/buttons/btn_membersonly.gif";
btn_membersonly_over = new Image(139, 24);
btn_membersonly_over.src = "/images/buttons/btn_membersonly_over.gif";

btn_otherclubs_off = new Image(139, 24);
btn_otherclubs_off.src = "/images/buttons/btn_otherclubs.gif";
btn_otherclubs_over = new Image(139, 24);
btn_otherclubs_over.src = "/images/buttons/btn_otherclubs_over.gif";

btn_vendors_off = new Image(139, 24);
btn_vendors_off.src = "/images/buttons/btn_vendors.gif";
btn_vendors_over = new Image(139, 24);
btn_vendors_over.src = "/images/buttons/btn_vendors_over.gif";

btn_clubapparel_off = new Image(139, 24);
btn_clubapparel_off.src = "/images/buttons/btn_clubapparel.gif";
btn_clubapparel_over = new Image(139, 24);
btn_clubapparel_over.src = "/images/buttons/btn_clubapparel_over.gif";

btn_classifieds_off = new Image(139, 24);
btn_classifieds_off.src = "/images/buttons/btn_classifieds.gif";
btn_classifieds_over = new Image(139, 24);
btn_classifieds_over.src = "/images/buttons/btn_classifieds_over.gif";

btn_techtalk_off = new Image(139, 24);
btn_techtalk_off.src = "/images/buttons/btn_techtalk.gif";
btn_techtalk_over = new Image(139, 24);
btn_techtalk_over.src = "/images/buttons/btn_techtalk_over.gif";

btn_photoalbums_off = new Image(139, 24);
btn_photoalbums_off.src = "/images/buttons/btn_photoalbums.gif";
btn_photoalbums_over = new Image(139, 24);
btn_photoalbums_over.src = "/images/buttons/btn_photoalbums_over.gif";

btn_meetings_off = new Image(139, 24);
btn_meetings_off.src = "/images/buttons/btn_meetings.gif";
btn_meetings_over = new Image(139, 24);
btn_meetings_over.src = "/images/buttons/btn_meetings_over.gif";

btn_events_off = new Image(139, 24);
btn_events_off.src = "/images/buttons/btn_events.gif";
btn_events_over = new Image(139, 24);
btn_events_over.src = "/images/buttons/btn_events_over.gif";

btn_contactus_off = new Image(139, 24);
btn_contactus_off.src = "/images/buttons/btn_contactus.gif";
btn_contactus_over = new Image(139, 24);
btn_contactus_over.src = "/images/buttons/btn_contactus_over.gif";

btn_joinus_off = new Image(139, 24);
btn_joinus_off.src = "/images/buttons/btn_joinus.gif";
btn_joinus_over = new Image(139, 24);
btn_joinus_over.src = "/images/buttons/btn_joinus_over.gif";

btn_clubinfo_off = new Image(139, 24);
btn_clubinfo_off.src = "/images/buttons/btn_clubinfo.gif";
btn_clubinfo_over = new Image(139, 24);
btn_clubinfo_over.src = "/images/buttons/btn_clubinfo_over.gif";

btn_home_off = new Image(139, 24);
btn_home_off.src = "/images/buttons/btn_home.gif";
btn_home_over = new Image(139, 24);
btn_home_over.src = "/images/buttons/btn_home_over.gif";

function ActiveOverImage(imageName) {
    imgActive = eval(imageName + "_over.src");
    document[imageName].src = imgActive;
}

function InActiveImage(imageName) {
    imgInActive = eval(imageName + "_off.src");
    document[imageName].src = imgInActive;
}

