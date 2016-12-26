$(document).ready(function(){
	// 초기 로드 시 선택된 Overlay가 없으므로 disable 시킨다.
	$(".setting *[data-flag]").attr("disabled", "disabled");
	$(".setting *[data-id]").attr("disabled", "disabled");
	
	$(".listhead .newbtn").click(function(){
		if($(".newwindow").css("display") == "none")
		{
			$(".disableall").show();
			$(".newwindow").show();
		}
		else
		{
			$(".disableall").hide();
			$(".newwindow").hide();
		}
	});

	$("*[data-flag=new-add]").click(function(){
		var html = "<div ";
		html+='data-url="'+$("*[data-flag=new-url]").val()+'" data-opacity="100" data-zoom="100" data-fps="30" data-x="0" data-y="0" data-width="300" data-height="300" data-clickthru="false" data-nonfocus="true" data-dragging="false" data-dragndrop="true" data-hide="false" data-resize="true"';
		html+="><span>"+$("*[data-flag=new-url]").val()+"</span></div>";
		$("*[data-flag=new-url]").val("about:blank");
		$(".list").append(html);
		$(".disableall").hide();
		$(".newwindow").hide();
		actAttach();
	});

	$("*[data-flag=overlay-delete]").click(function(){
		$(".setting *[data-flag]").attr("disabled", "disabled");
		$(".setting *[data-id]").attr("disabled", "disabled");
		$(".setting *[data-flag]").each(function(){
			if($(this).is("[data-checked]"))
			{
				$(this).attr("data-checked", "false");
			}
			else
			{
				$(this).val("");
			}
		});
		$($(".list div")[parseInt($(".list").attr("data-selected-index"))]).remove();
		$(".list").attr("data-selected-index", "-1");
	});

	$("*[data-flag=overlay-save]").click(function(){
		var savedvar = [
			"url",
			"opacity",
			"zoom",
			"fps",
			"clickthru",
			"nonfocus",
			"dragging",
			"dragndrop",
			"hide",
			"resize",
			"x",
			"y",
			"width",
			"height"
		];
		var index = parseInt($(".list").attr("data-selected-index"));
		$($(".list div")[index]).find("span").html($("*[data-flag=overlay-title]").val());
		for(var i in savedvar)
		{
			if($("*[data-flag=overlay-"+savedvar[i]+"]").is("[data-checked]"))
			{
				$($(".list div")[index]).attr("data-"+savedvar[i], $("*[data-flag=overlay-"+savedvar[i]+"]").attr("data-checked")=="true"?"true":"false");
			}
			else
			{
				$($(".list div")[index]).attr("data-"+savedvar[i], $("*[data-flag=overlay-"+savedvar[i]+"]").val());
			}
		}
	});

	$("*[data-checked]").click(function(){
		if($(this).is("[disabled]")) return;
		if($(this).parent().is("[disabled]")) return;

		$(this).attr("data-checked", $(this).attr("data-checked")=="true"?"false":"true");
		if($(this).is("[data-status-off]") && $(this).attr("data-checked") == "false")
		{
			$(this).html($(this).attr("data-status-off"));
			if($(this).is("[data-status-off-css]"))
			{
				$(this).css(JSON.parse($(this).attr("data-status-off-css").replace(/'/ig, "\"")));
			}
		}
		else if($(this).is("[data-status-on]") && $(this).attr("data-checked") == "true")
		{
			$(this).html($(this).attr("data-status-on"));
			if($(this).is("[data-status-on-css]"))
			{
				$(this).css(JSON.parse($(this).attr("data-status-on-css").replace(/'/ig, "\"")));
			}
		}

		if($(this).is("[data-disableobject]"))
		{
			if($(this).attr("data-checked")=="true")
			{
				var obj = JSON.parse($(this).attr("data-disableobject").replace(/'/ig, "\""));
				for(var i in obj.objects)
				{
					$("*[data-id=\""+obj.objects[i]+"\"]").attr("disabled","disabled");
					$("*[data-flag=\""+obj.objects[i]+"\"]").attr("disabled","disabled");
				}
			}
			else
			{
				var obj = JSON.parse($(this).attr("data-disableobject").replace(/'/ig, "\""));
				for(var i in obj.objects)
				{
					$("*[data-id=\""+obj.objects[i]+"\"]").removeAttr("disabled");
					$("*[data-flag=\""+obj.objects[i]+"\"]").removeAttr("disabled");
				}
			}
		}
	});

	$("input[type=range]").change(function(){
		$(this).parent().parent().find(".valueview").html($(this).val()+$(this).attr("data-str"));
	});

	actAttach();
});

function actAttach()
{
	// 파일 선택 목록, list 아이템이 추가될 때 한번씩 호출하면 되는 함수
	$(".lists>div>.select").click(function(){
		$("*[data-flag=new-url]").val($(".lists").attr("data-parent")+$(this).parent().text());
		$("*[data-flag=new-url]").focus();
	});

	$(".list div").click(function(){
		$(".list div").each(function(){$(this).removeClass("selected");});
		$(this).addClass("selected");
		var i = 0;
		$(".list div").each(function(){
			if($(this).hasClass("selected")) $(".list").attr("data-selected-index", i);
			i++;
		});
		$("*[data-id]").removeAttr("disabled");
		$("*[data-flag]").removeAttr("disabled");

		$(".setting *[data-flag=overlay-title]").val($(this).find("span").html());
		$.each(this.attributes, function(){
			if(this.specified)
			{
				if($(".setting *[data-flag=\""+this.name.replace("data", "overlay")+"\"]").is("[data-checked]"))
				{
					$(".setting *[data-flag=\""+this.name.replace("data", "overlay")+"\"]").attr("data-checked", this.value);
				}
				else
				{
					$(".setting *[data-flag=\""+this.name.replace("data", "overlay")+"\"]").val(this.value.replace(/%20/ig, " "));
					$(".setting *[data-flag=\""+this.name.replace("data", "overlay")+"\"]").parent().parent().find(".valueview").html($(".setting *[data-flag=\""+this.name.replace("data", "overlay")+"\"]").val()+$(".setting *[data-flag=\""+this.name.replace("data", "overlay")+"\"]").attr("data-str"));
				}
			}
		});
	});
}