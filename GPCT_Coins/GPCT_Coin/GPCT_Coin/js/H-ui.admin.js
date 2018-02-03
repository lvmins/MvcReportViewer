/*H-ui.admin.js v2.3.1 date:15:42 2015.08.19 by:guojunhui*/
/*获取顶部选项卡总长度*/
function tabNavallwidth() {
    var taballwidth = 0,
		$tabNav = $(".acrossTab"),
		$tabNavWp = $(".Hui-tabNav-wp"),
		$tabNavitem = $(".acrossTab li"),
		$tabNavmore = $(".Hui-tabNav-more");
    if (!$tabNav[0]) { return }
    $tabNavitem.each(function (index, element) {
        taballwidth += Number(parseFloat($(this).width() + 60))
    });
    $tabNav.width(taballwidth + 25);
    var w = $tabNavWp.width();
    if (taballwidth + 25 > w) {
        $tabNavmore.show()
    }
    else {
        $tabNavmore.hide();
        $tabNav.css({ left: 0 })
    }
}

/*左侧菜单响应式*/
function Huiasidedisplay() {
    if ($(window).width() >= 768) {
        $(".Hui-aside").show()
    }
}
function getskincookie() {
    var v = getCookie("Huiskin");
    if (v == null || v == "") {
        v = "default";
    }
    $("#skin").attr("href", "/skin/" + v + "/skin.css");
}
$(function () {
    getskincookie();
    //layer.config({extend: 'extend/layer.ext.js'});
    Huiasidedisplay();
    var resizeID;
    $(window).resize(function () {
        clearTimeout(resizeID);
        resizeID = setTimeout(function () {
            Huiasidedisplay();
        }, 500);
    });

    $(".Hui-nav-toggle").click(function () {
        $(".Hui-aside").slideToggle();
    });
    $(".Hui-aside").on("click", ".menu_dropdown dd li a", function () {
        if ($(window).width() < 768) {
            $(".Hui-aside").slideToggle();
        }
    });
    /*左侧菜单*/
    $.Huifold(".menu_dropdown dl dt", ".menu_dropdown dl dd", "fast", 1, "click");
    /*选项卡导航*/

    $(".Hui-aside").on("click", ".menu_dropdown a", function () {
        if ($(this).attr('_href')) {
            var bStop = false;
            var bStopIndex = 0;
            var _href = $(this).attr('_href');
            var _titleName = $(this).html();
            var topWindow = $(window.parent.document);
            var show_navLi = topWindow.find("#min_title_list li");
            show_navLi.each(function () {
                if ($(this).find('span').attr("data-href") == _href) {
                    bStop = true;
                    bStopIndex = show_navLi.index($(this));
                    return false;
                }
            });
            if (!bStop) {
                creatIframe(_href, _titleName);
                //min_titleList();
            }
            else {
                show_navLi.removeClass("active").eq(bStopIndex).addClass("active");
                var iframe_box = topWindow.find("#iframe_box");
                iframe_box.find(".show_iframe").hide().eq(bStopIndex).show().find("iframe").attr("src", _href);
            }
        }
    });

    function creatIframe(href, titleName) {
        var topWindow = $(window.parent.document);
        var show_nav = topWindow.find('#min_title_list');
        show_nav.find('li').removeClass("active");
        var iframe_box = topWindow.find('#iframe_box');
        show_nav.append('<li class="active"><span data-href="' + href + '">' + titleName + '</span><i></i><em></em></li>');
        tabNavallwidth();
        var iframeBox = iframe_box.find('.show_iframe');
        iframeBox.hide();
        iframe_box.append('<div class="show_iframe"><div class="loading"></div><iframe frameborder="0" src=' + href + '></iframe></div>');
        var showBox = iframe_box.find('.show_iframe:visible');
        showBox.find('iframe').attr("src", href).load(function () {
            showBox.find('.loading').hide();
        });
    }

    var num = 0;
    var oUl = $("#min_title_list");
    var hide_nav = $("#Hui-tabNav");
    $(document).on("click", "#min_title_list li", function () {
        var bStopIndex = $(this).index();
        var iframe_box = $("#iframe_box");
        $("#min_title_list li").removeClass("active").eq(bStopIndex).addClass("active");
        iframe_box.find(".show_iframe").hide().eq(bStopIndex).show();
    });
    $(document).on("click", "#min_title_list li i", function () {
        var aCloseIndex = $(this).parents("li").index();
        $(this).parent().remove();
        $('#iframe_box').find('.show_iframe').eq(aCloseIndex).remove();
        num == 0 ? num = 0 : num--;
        tabNavallwidth();
    });


    /*换肤*/
    $("#Hui-skin .dropDown-menu a").click(function () {
        var v = $(this).attr("data-val");
        setCookie("Huiskin", v);
        $("#skin").attr("href", "skin/" + v + "/skin.css");
    });
});




/*-----    选项卡    ----------*/

/*下拉菜单*/
$(document).on("mouseenter", ".dropDown", function () {
    $(this).addClass("hover");
});
$(document).on("mouseleave", ".dropDown", function () {
    $(this).removeClass("hover");
});
$(document).on("mouseenter", ".dropDown_hover", function () {
    $(this).addClass("open");
});
$(document).on("mouseleave", ".dropDown_hover", function () {
    $(this).removeClass("open");
});
$(document).on("click", ".dropDown-menu li a", function () {
    $(".dropDown").removeClass('open');
});
$(document).on('click', function (event) {
    var e_t = $(event.target).parents('.dropDown_click');
    if ($(".dropDown_click").hasClass('open')) {
        if (e_t.hasClass('open')) {
            e_t.removeClass('open');
            return;
        }
        $(".dropDown_click").removeClass('open');
        e_t.toggleClass('open');
    } else {
        e_t.toggleClass('open');
    }
});

/*hover*/
jQuery.Huihover = function (obj) {
    $(obj).hover(function () { $(this).addClass("hover"); }, function () { $(this).removeClass("hover"); });
};
/*得到失去焦点*/
jQuery.Huifocusblur = function (obj) {
    $(obj).focus(function () { $(this).addClass("focus").removeClass("inputError"); });
    $(obj).blur(function () { $(this).removeClass("focus"); });
};


/*左侧菜单-隐藏显示*/
function displaynavbar(obj) {
    if ($(obj).hasClass("open")) {
        $(obj).removeClass("open");
        $("body").removeClass("big-page");
    } else {
        $(obj).addClass("open");
        $("body").addClass("big-page");

    }
}

/*折叠*/
jQuery.Huifold = function (obj, obj_c, speed, obj_type, Event) {
    if (obj_type == 2) {
        $(obj + ":first").find("b").html("-");
        $(obj_c + ":first").show();
    }
    $(obj).on(Event, function () {
        if ($(this).next().is(":visible")) {
            if (obj_type == 2) {
                return false;
            } else {
                $(this).next().slideUp(speed).end().removeClass("selected");
                if ($(this).find("b")) {
                    $(this).find("b").html("+");
                }
            }
        }
        else {
            if (obj_type == 3) {
                $(this).next().slideDown(speed).end().addClass("selected");
                if ($(this).find("b")) {
                    $(this).find("b").html("-");
                }
            } else {
                $(obj_c).slideUp(speed);
                $(obj).removeClass("selected");
                if ($(this).find("b")) {
                    $(obj).find("b").html("+");
                }
                $(this).next().slideDown(speed).end().addClass("selected");
                if ($(this).find("b")) {
                    $(this).find("b").html("-");
                }
            }
        }
    });
}

/*设置cookie*/
function setCookie(name, value, Days) {
    if (Days == null || Days == '') {
        Days = 300;
    }
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + "; path=/;expires=" + exp.toGMTString();
}

/*获取cookie*/
function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}

$(function () {
    /*****表单*****/
    $.Huifocusblur(".input-text,.textarea");
    /*按钮loading*/
    $('.btn-loading').click(function () {
        var $btn = $(this);
        var btnval = $btn.val();
        $btn.addClass("disabled").val("loading").attr("disabled", "disabled");
        setTimeout(function () {
            $btn.removeClass("disabled").val(btnval).removeAttr("disabled");
        }, 3000);
    });
    /**/
});

function setTimeoutReplace() {
    setTimeout(function () {
        location.replace(location.href);
    }, 1000);
}