/*!
 * jQuery.filer minified
 * Copyright (c) 2016 CreativeDream
 * Website: https://github.com/CreativeDream/jquery.filer
 * Version: 1.3 (14-Sep-2016)
 * Requires: jQuery v1.7.1 or later
 */
!function(a){"use strict";a.fn.filer=function(b){return this.each(function(c,d){var e=a(d),f=".jFiler",g=a(),h=a(),i=a(),j=[],k=a.isFunction(b)?b(e,a.fn.filer.defaults):b,l=k&&a.isPlainObject(k)?a.extend(!0,{},a.fn.filer.defaults,k):a.fn.filer.defaults,m={init:function(){e.wrap('<div class="jFiler"></div>'),m._set("props"),e.prop("jFiler").boxEl=g=e.closest(f),m._changeInput()},_bindInput:function(){l.changeInput&&h.length>0&&h.on("click",m._clickHandler),e.on({focus:function(){h.addClass("focused")},blur:function(){h.removeClass("focused")},change:m._onChange}),l.dragDrop&&(l.dragDrop.dragContainer.on("drag dragstart dragend dragover dragenter dragleave drop",function(a){a.preventDefault(),a.stopPropagation()}),l.dragDrop.dragContainer.on("drop",m._dragDrop.drop),l.dragDrop.dragContainer.on("dragover",m._dragDrop.dragEnter),l.dragDrop.dragContainer.on("dragleave",m._dragDrop.dragLeave)),l.uploadFile&&l.clipBoardPaste&&a(window).on("paste",m._clipboardPaste)},_unbindInput:function(b){l.changeInput&&h.length>0&&h.off("click",m._clickHandler),b&&(e.off("change",m._onChange),l.dragDrop&&(l.dragDrop.dragContainer.off("drop",m._dragDrop.drop),l.dragDrop.dragContainer.off("dragover",m._dragDrop.dragEnter),l.dragDrop.dragContainer.off("dragleave",m._dragDrop.dragLeave)),l.uploadFile&&l.clipBoardPaste&&a(window).off("paste",m._clipboardPaste))},_clickHandler:function(){if(!l.uploadFile&&l.addMore&&0!=e.val().length){m._unbindInput(!0);var b=a('<input type="file" />'),c=e.prop("attributes");a.each(c,function(){"required"!=this.name&&b.attr(this.name,this.value)}),e.after(b),j.push(b),e=b,m._bindInput(),m._set("props")}e.click()},_applyAttrSettings:function(){var a=["name","limit","maxSize","fileMaxSize","extensions","changeInput","showThumbs","appendTo","theme","addMore","excludeName","files","uploadUrl","uploadData","options"];for(var b in a){var c="data-jfiler-"+a[b];if(m._assets.hasAttr(c)){switch(a[b]){case"changeInput":case"showThumbs":case"addMore":l[a[b]]=["true","false"].indexOf(e.attr(c))>-1?"true"==e.attr(c):e.attr(c);break;case"extensions":l[a[b]]=e.attr(c).replace(/ /g,"").split(",");break;case"uploadUrl":l.uploadFile&&(l.uploadFile.url=e.attr(c));break;case"uploadData":l.uploadFile&&(l.uploadFile.data=JSON.parse(e.attr(c)));break;case"files":case"options":l[a[b]]=JSON.parse(e.attr(c));break;default:l[a[b]]=e.attr(c)}e.removeAttr(c)}}},_changeInput:function(){if(m._applyAttrSettings(),null!=l.beforeRender&&"function"==typeof l.beforeRender?l.beforeRender(g,e):null,l.theme&&g.addClass("jFiler-theme-"+l.theme),"input"!=e.get(0).tagName.toLowerCase()&&"file"!=e.get(0).type)h=e,e=a('<input type="file" name="'+l.name+'" />'),e.css({position:"absolute",left:"-9999px",top:"-9999px","z-index":"-9999"}),g.prepend(e),m._isGn=e;else if(l.changeInput){switch(typeof l.changeInput){case"boolean":h=a('<div class="jFiler-input"><div class="jFiler-input-caption"><span>'+l.captions.feedback+'</span></div><div class="jFiler-input-button">'+l.captions.button+'</div></div>"');break;case"string":case"object":h=a(l.changeInput);break;case"function":h=a(l.changeInput(g,e,l))}e.after(h),e.css({position:"absolute",left:"-9999px",top:"-9999px","z-index":"-9999"})}e.prop("jFiler").newInputEl=h,l.dragDrop&&(l.dragDrop.dragContainer=l.dragDrop.dragContainer?a(l.dragDrop.dragContainer):h),(!l.limit||l.limit&&l.limit>=2)&&(e.attr("multiple","multiple"),"[]"!=e.attr("name").slice(-2)?e.attr("name",e.attr("name")+"[]"):null),e.attr("disabled")||l.disabled?(l.disabled=!0,m._unbindInput(!0),g.addClass("jFiler-disabled")):(l.disabled=!1,m._bindInput(),g.removeClass("jFiler-disabled")),l.files&&m._append(!1,{files:l.files}),null!=l.afterRender&&"function"==typeof l.afterRender?l.afterRender(i,g,h,e):null},_clear:function(){m.files=null,e.prop("jFiler").files=null,l.uploadFile||l.addMore||m._reset(),m._set("feedback",m._itFl&&m._itFl.length>0?m._itFl.length+" "+l.captions.feedback2:l.captions.feedback),null!=l.onEmpty&&"function"==typeof l.onEmpty?l.onEmpty(g,h,e):null},_reset:function(b){if(!b){if(!l.uploadFile&&l.addMore){for(var c=0;c<j.length;c++)j[c].remove();j=[],m._unbindInput(!0),e=m._isGn?m._isGn:a(d),m._bindInput()}m._set("input","")}m._itFl=[],m._itFc=null,m._ajFc=0,m._set("props"),e.prop("jFiler").files_list=m._itFl,e.prop("jFiler").current_file=m._itFc,m._itFr=[],g.find("input[name^='jfiler-items-exclude-']:hidden").remove(),i.fadeOut("fast",function(){a(this).remove()}),e.prop("jFiler").listEl=i=a()},_set:function(a,b){switch(a){case"input":e.val(b);break;case"feedback":h.length>0&&h.find(".jFiler-input-caption span").html(b);break;case"props":e.prop("jFiler")||e.prop("jFiler",{options:l,listEl:i,boxEl:g,newInputEl:h,inputEl:e,files:m.files,files_list:m._itFl,current_file:m._itFc,append:function(a){return m._append(!1,{files:[a]})},enable:function(){l.disabled&&(l.disabled=!1,e.removeAttr("disabled"),g.removeClass("jFiler-disabled"),m._bindInput())},disable:function(){l.disabled||(l.disabled=!0,g.addClass("jFiler-disabled"),m._unbindInput(!0))},remove:function(a){return m._remove(null,{binded:!0,data:{id:a}}),!0},reset:function(){return m._reset(),m._clear(),!0},retry:function(a){return m._retryUpload(a)}})}},_filesCheck:function(){var b=0;if(l.limit&&m.files.length+m._itFl.length>l.limit)return l.dialogs.alert(m._assets.textParse(l.captions.errors.filesLimit)),!1;for(var c=0;c<m.files.length;c++){var d=m.files[c],e=d.name.split(".").pop().toLowerCase(),f={name:d.name,size:d.size,size2:m._assets.bytesToSize(d.size),type:d.type,ext:e};if(null!=l.extensions&&a.inArray(e,l.extensions)==-1&&a.inArray(f.type,l.extensions)==-1)return l.dialogs.alert(m._assets.textParse(l.captions.errors.filesType,f)),!1;if(null!=l.maxSize&&m.files[c].size>1048576*l.maxSize||null!=l.fileMaxSize&&m.files[c].size>1048576*l.fileMaxSize)return l.dialogs.alert(m._assets.textParse(l.captions.errors.filesSize,f)),!1;if(4096==d.size&&0==d.type.length)return l.dialogs.alert(m._assets.textParse(l.captions.errors.folderUpload,f)),!1;if(null!=l.onFileCheck&&"function"==typeof l.onFileCheck?l.onFileCheck(f,l,m._assets.textParse)===!1:null)return!1;if((l.uploadFile||l.addMore)&&!l.allowDuplicates){var f=m._itFl.filter(function(a,b){if(a.file.name==d.name&&a.file.size==d.size&&a.file.type==d.type&&(!d.lastModified||a.file.lastModified==d.lastModified))return!0});if(f.length>0){if(1==m.files.length)return!1;d._pendRemove=!0}}b+=m.files[c].size}return!(null!=l.maxSize&&b>=Math.round(1048576*l.maxSize))||(l.dialogs.alert(m._assets.textParse(l.captions.errors.filesSizeAll)),!1)},_thumbCreator:{create:function(b){var c=m.files[b],d=m._itFc?m._itFc.id:b,e=c.name,f=c.size,g=c.file,h=c.type?c.type.split("/",1):"".toString().toLowerCase(),j=e.indexOf(".")!=-1?e.split(".").pop().toLowerCase():"",k=l.uploadFile?'<div class="jFiler-jProgressBar">'+l.templates.progressBar+"</div>":"",n={id:d,name:e,size:f,size2:m._assets.bytesToSize(f),url:g,type:h,extension:j,icon:m._assets.getIcon(j,h),icon2:m._thumbCreator.generateIcon({type:h,extension:j}),image:'<div class="jFiler-item-thumb-image fi-loading"></div>',progressBar:k,_appended:c._appended},o="";return c.opts&&(n=a.extend({},c.opts,n)),o=a(m._thumbCreator.renderContent(n)).attr("data-jfiler-index",d),o.get(0).jfiler_id=d,m._thumbCreator.renderFile(c,o,n),c.forList?o:(m._itFc.html=o,o.hide()[l.templates.itemAppendToEnd?"appendTo":"prependTo"](i.find(l.templates._selectors.list)).show(),void(c._appended||m._onSelect(b)))},renderContent:function(a){return m._assets.textParse(a._appended?l.templates.itemAppend:l.templates.item,a)},renderFile:function(b,c,d){if(0==c.find(".jFiler-item-thumb-image").length)return!1;if(b.file&&"image"==d.type){var e='<img src="'+b.file+'" draggable="false" />',f=c.find(".jFiler-item-thumb-image.fi-loading");return a(e).error(function(){e=m._thumbCreator.generateIcon(d),c.addClass("jFiler-no-thumbnail"),f.removeClass("fi-loading").html(e)}).load(function(){f.removeClass("fi-loading").html(e)}),!0}if(window.File&&window.FileList&&window.FileReader&&"image"==d.type&&d.size<1e7){var g=new FileReader;g.onload=function(a){var b=c.find(".jFiler-item-thumb-image.fi-loading");if(l.templates.canvasImage){var e=document.createElement("canvas"),f=e.getContext("2d"),g=new Image;g.onload=function(){var a=b.height(),c=b.width(),d=g.height/a,h=g.width/c,i=d<h?d:h,j=g.height/i,k=g.width/i,l=Math.ceil(Math.log(g.width/k)/Math.log(2));if(e.height=a,e.width=c,g.width<e.width||g.height<e.height||l<=1){var m=g.width<e.width?e.width/2-g.width/2:g.width>e.width?-(g.width-e.width)/2:0,n=g.height<e.height?e.height/2-g.height/2:0;f.drawImage(g,m,n,g.width,g.height)}else{var o=document.createElement("canvas"),p=o.getContext("2d");o.width=.5*g.width,o.height=.5*g.height,p.fillStyle="#fff",p.fillRect(0,0,o.width,o.height),p.drawImage(g,0,0,o.width,o.height),p.drawImage(o,0,0,.5*o.width,.5*o.height),f.drawImage(o,k>e.width?k-e.width:0,0,.5*o.width,.5*o.height,0,0,k,j)}b.removeClass("fi-loading").html('<img src="'+e.toDataURL("image/png")+'" draggable="false" />')},g.onerror=function(){c.addClass("jFiler-no-thumbnail"),b.removeClass("fi-loading").html(m._thumbCreator.generateIcon(d))},g.src=a.target.result}else b.removeClass("fi-loading").html('<img src="'+a.target.result+'" draggable="false" />')},g.readAsDataURL(b)}else{var e=m._thumbCreator.generateIcon(d),f=c.find(".jFiler-item-thumb-image.fi-loading");c.addClass("jFiler-no-thumbnail"),f.removeClass("fi-loading").html(e)}},generateIcon:function(b){var c=new Array(3);if(b&&b.type&&b.type[0]&&b.extension)switch(b.type[0]){case"image":c[0]="f-image",c[1]='<i class="icon-jfi-file-image"></i>';break;case"video":c[0]="f-video",c[1]='<i class="icon-jfi-file-video"></i>';break;case"audio":c[0]="f-audio",c[1]='<i class="icon-jfi-file-audio"></i>';break;default:c[0]="f-file f-file-ext-"+b.extension,c[1]=b.extension.length>0?"."+b.extension:"",c[2]=1}else c[0]="f-file",c[1]=b.extension&&b.extension.length>0?"."+b.extension:"",c[2]=1;var d='<span class="jFiler-icon-file '+c[0]+'">'+c[1]+"</span>";if(1==c[2]){var e=m._assets.text2Color(b.extension);if(e){var f=a(d).appendTo("body");f.css("background-color",m._assets.text2Color(b.extension)),d=f.prop("outerHTML"),f.remove()}}return d},_box:function(b){if(null!=l.beforeShow&&"function"==typeof l.beforeShow&&!l.beforeShow(m.files,i,g,h,e))return!1;if(i.length<1){if(l.appendTo)var c=a(l.appendTo);else var c=g;c.find(".jFiler-items").remove(),i=a('<div class="jFiler-items jFiler-row"></div>'),e.prop("jFiler").listEl=i,i.append(m._assets.textParse(l.templates.box)).appendTo(c),i.on("click",l.templates._selectors.remove,function(c){c.preventDefault();var d=[b?b.remove.event:c,b?b.remove.el:a(this).closest(l.templates._selectors.item)],e=function(a){m._remove(d[0],d[1])};l.templates.removeConfirmation?l.dialogs.confirm(l.captions.removeConfirmation,e):e()})}for(var d=0;d<m.files.length;d++)m.files[d]._appended||(m.files[d]._choosed=!0),m._addToMemory(d),m._thumbCreator.create(d)}},_upload:function(b){var c=m._itFl[b],d=c.html,f=new FormData;if(f.append(e.attr("name"),c.file,!!c.file.name&&c.file.name),null!=l.uploadFile.data&&a.isPlainObject("function"==typeof l.uploadFile.data?l.uploadFile.data(c.file):l.uploadFile.data))for(var g in l.uploadFile.data)f.append(g,l.uploadFile.data[g]);m._ajax.send(d,f,c)},_ajax:{send:function(b,c,d){return d.ajax=a.ajax({url:l.uploadFile.url,data:c,type:l.uploadFile.type,enctype:l.uploadFile.enctype,xhr:function(){var c=a.ajaxSettings.xhr();return c.upload&&c.upload.addEventListener("progress",function(a){m._ajax.progressHandling(a,b)},!1),c},complete:function(a,b){d.ajax=!1,m._ajFc++,l.uploadFile.synchron&&d.id+1<m._itFl.length&&m._upload(d.id+1),m._ajFc>=m.files.length&&(m._ajFc=0,e.get(0).value="",null!=l.uploadFile.onComplete&&"function"==typeof l.uploadFile.onComplete?l.uploadFile.onComplete(i,g,h,e,a,b):null)},beforeSend:function(a,c){return null==l.uploadFile.beforeSend||"function"!=typeof l.uploadFile.beforeSend||l.uploadFile.beforeSend(b,i,g,h,e,d.id,a,c)},success:function(a,c,f){d.uploaded=!0,null!=l.uploadFile.success&&"function"==typeof l.uploadFile.success?l.uploadFile.success(a,b,i,g,h,e,d.id,c,f):null},error:function(a,c,f){d.uploaded=!1,null!=l.uploadFile.error&&"function"==typeof l.uploadFile.error?l.uploadFile.error(b,i,g,h,e,d.id,a,c,f):null},statusCode:l.uploadFile.statusCode,cache:!1,contentType:!1,processData:!1}),d.ajax},progressHandling:function(a,b){if(a.lengthComputable){var c=Math.round(100*a.loaded/a.total).toString();null!=l.uploadFile.onProgress&&"function"==typeof l.uploadFile.onProgress?l.uploadFile.onProgress(c,b,i,g,h,e):null,b.find(".jFiler-jProgressBar").find(l.templates._selectors.progressBar).css("width",c+"%")}}},_dragDrop:{dragEnter:function(a){clearTimeout(m._dragDrop._drt),l.dragDrop.dragContainer.addClass("dragged"),m._set("feedback",l.captions.drop),null!=l.dragDrop.dragEnter&&"function"==typeof l.dragDrop.dragEnter?l.dragDrop.dragEnter(a,h,e,g):null},dragLeave:function(a){clearTimeout(m._dragDrop._drt),m._dragDrop._drt=setTimeout(function(a){return m._dragDrop._dragLeaveCheck(a)?(l.dragDrop.dragContainer.removeClass("dragged"),m._set("feedback",l.captions.feedback),void(null!=l.dragDrop.dragLeave&&"function"==typeof l.dragDrop.dragLeave?l.dragDrop.dragLeave(a,h,e,g):null)):(m._dragDrop.dragLeave(a),!1)},100,a)},drop:function(a){clearTimeout(m._dragDrop._drt),l.dragDrop.dragContainer.removeClass("dragged"),m._set("feedback",l.captions.feedback),a&&a.originalEvent&&a.originalEvent.dataTransfer&&a.originalEvent.dataTransfer.files&&a.originalEvent.dataTransfer.files.length>0&&m._onChange(a,a.originalEvent.dataTransfer.files),null!=l.dragDrop.drop&&"function"==typeof l.dragDrop.drop?l.dragDrop.drop(a.originalEvent.dataTransfer.files,a,h,e,g):null},_dragLeaveCheck:function(b){var c=a(b.currentTarget),d=0;return!(!c.is(h)&&(d=h.find(c).length,d>0))}},_clipboardPaste:function(a,b){if((b||a.originalEvent.clipboardData||a.originalEvent.clipboardData.items)&&(!b||a.originalEvent.dataTransfer||a.originalEvent.dataTransfer.items)&&!m._clPsePre){var c=b?a.originalEvent.dataTransfer.items:a.originalEvent.clipboardData.items,d=function(a,b,c){b=b||"",c=c||512;for(var d=atob(a),e=[],f=0;f<d.length;f+=c){for(var g=d.slice(f,f+c),h=new Array(g.length),i=0;i<g.length;i++)h[i]=g.charCodeAt(i);var j=new Uint8Array(h);e.push(j)}var k=new Blob(e,{type:b});return k};if(c)for(var e=0;e<c.length;e++)if(c[e].type.indexOf("image")!==-1||c[e].type.indexOf("text/uri-list")!==-1){if(b)try{window.atob(a.originalEvent.dataTransfer.getData("text/uri-list").toString().split(",")[1])}catch(a){return}var f=b?d(a.originalEvent.dataTransfer.getData("text/uri-list").toString().split(",")[1],"image/png"):c[e].getAsFile();f.name=Math.random().toString(36).substring(5),f.name+=f.type.indexOf("/")!=-1?"."+f.type.split("/")[1].toString().toLowerCase():".png",m._onChange(a,[f]),m._clPsePre=setTimeout(function(){delete m._clPsePre},1e3)}}},_onSelect:function(b){l.uploadFile&&!a.isEmptyObject(l.uploadFile)&&(!l.uploadFile.synchron||l.uploadFile.synchron&&0==a.grep(m._itFl,function(a){return a.ajax}).length)&&m._upload(m._itFc.id),m.files[b]._pendRemove&&(m._itFc.html.hide(),m._remove(null,{binded:!0,data:{id:m._itFc.id}})),null!=l.onSelect&&"function"==typeof l.onSelect?l.onSelect(m.files[b],m._itFc.html,i,g,h,e):null,b+1>=m.files.length&&(null!=l.afterShow&&"function"==typeof l.afterShow?l.afterShow(i,g,h,e):null)},_onChange:function(b,c){if(c){if(!c||0==c.length)return m._set("input",""),m._clear(),!1;m.files=c}else{if(!e.get(0).files||"undefined"==typeof e.get(0).files||0==e.get(0).files.length)return l.uploadFile||l.addMore||(m._set("input",""),m._clear()),!1;m.files=e.get(0).files}if(l.uploadFile||l.addMore||m._reset(!0),e.prop("jFiler").files=m.files,!m._filesCheck()||null!=l.beforeSelect&&"function"==typeof l.beforeSelect&&!l.beforeSelect(m.files,i,g,h,e))return m._set("input",""),m._clear(),l.addMore&&j.length>0&&(m._unbindInput(!0),j[j.length-1].remove(),j.splice(j.length-1,1),e=j.length>0?j[j.length-1]:a(d),m._bindInput()),!1;if(m._set("feedback",m.files.length+m._itFl.length+" "+l.captions.feedback2),l.showThumbs)m._thumbCreator._box();else for(var f=0;f<m.files.length;f++)m.files[f]._choosed=!0,m._addToMemory(f),m._onSelect(f)},_append:function(a,b){var c=!!b&&b.files;if(c&&!(c.length<=0)&&(m.files=c,e.prop("jFiler").files=m.files,l.showThumbs)){for(var d=0;d<m.files.length;d++)m.files[d]._appended=!0;m._thumbCreator._box()}},_getList:function(a,b){var c=!!b&&b.files;if(c&&!(c.length<=0)&&(m.files=c,e.prop("jFiler").files=m.files,l.showThumbs)){for(var d=[],f=0;f<m.files.length;f++)m.files[f].forList=!0,d.push(m._thumbCreator.create(f));b.callback&&b.callback(d,i,g,h,e)}},_retryUpload:function(b,c){var d=parseInt("object"==typeof c?c.attr("data-jfiler-index"):c),f=m._itFl.filter(function(a,b){return a.id==d});return f.length>0&&(!l.uploadFile||a.isEmptyObject(l.uploadFile)||f[0].uploaded?void 0:(m._itFc=f[0],e.prop("jFiler").current_file=m._itFc,m._upload(d),!0))},_remove:function(b,d){if(d.binded){if("undefined"!=typeof d.data.id&&(d=i.find(l.templates._selectors.item+"[data-jfiler-index='"+d.data.id+"']"),0==d.length))return!1;d.data.el&&(d=d.data.el)}var f=function(b){var d=g.find("input[name^='jfiler-items-exclude-']:hidden").first();0==d.length&&(d=a('<input type="hidden" name="jfiler-items-exclude-'+(l.excludeName?l.excludeName:("[]"!=e.attr("name").slice(-2)?e.attr("name"):e.attr("name").substring(0,e.attr("name").length-2))+"-"+c)+'">'),d.appendTo(g)),b&&a.isArray(b)&&(b=JSON.stringify(b),d.val(b))},j=function(b,c){var d=m._itFl[c],e=[];if(d.file._choosed||d.file._appended||d.uploaded){m._itFr.push(d);for(var g=m._itFl.filter(function(a){return a.file.name==d.file.name}),h=0;h<m._itFr.length;h++)l.addMore&&m._itFr[h]==d&&g.length>0&&(m._itFr[h].remove_name=g.indexOf(d)+"://"+m._itFr[h].file.name),e.push(m._itFr[h].remove_name?m._itFr[h].remove_name:m._itFr[h].file.name)}f(e),m._itFl.splice(c,1),m._itFl.length<1?(m._reset(),m._clear()):m._set("feedback",m._itFl.length+" "+l.captions.feedback2),b.fadeOut("fast",function(){a(this).remove()})},k=d.get(0).jfiler_id||d.attr("data-jfiler-index"),n=null;for(var o in m._itFl)"length"!==o&&m._itFl.hasOwnProperty(o)&&m._itFl[o].id==k&&(n=o);return!!m._itFl.hasOwnProperty(n)&&(m._itFl[n].ajax?(m._itFl[n].ajax.abort(),void j(d,n)):void(null!=l.onRemove&&"function"==typeof l.onRemove&&l.onRemove(d,m._itFl[n].file,n,i,g,h,e)===!1||j(d,n)))},_addToMemory:function(b){m._itFl.push({id:m._itFl.length,file:m.files[b],html:a(),ajax:!1,uploaded:!1}),(l.addMore||m.files[b]._appended)&&(m._itFl[m._itFl.length-1].input=e),m._itFc=m._itFl[m._itFl.length-1],e.prop("jFiler").files_list=m._itFl,e.prop("jFiler").current_file=m._itFc},_assets:{bytesToSize:function(a){if(0==a)return"0 Byte";var b=1e3,c=["Bytes","KB","MB","GB","TB","PB","EB","ZB","YB"],d=Math.floor(Math.log(a)/Math.log(b));return(a/Math.pow(b,d)).toPrecision(3)+" "+c[d]},hasAttr:function(a,b){var b=b?b:e,c=b.attr(a);return!(!c||"undefined"==typeof c)},getIcon:function(b,c){var d=["audio","image","text","video"];return a.inArray(c,d)>-1?'<i class="icon-jfi-file-'+c+" jfi-file-ext-"+b+'"></i>':'<i class="icon-jfi-file-o jfi-file-type-'+c+" jfi-file-ext-"+b+'"></i>'},textParse:function(b,c){switch(c=a.extend({},{limit:l.limit,maxSize:l.maxSize,fileMaxSize:l.fileMaxSize,extensions:l.extensions?l.extensions.join(","):null},c&&a.isPlainObject(c)?c:{},l.options),typeof b){case"string":return b.replace(/\{\{fi-(.*?)\}\}/g,function(a,b){return b=b.replace(/ /g,""),b.match(/(.*?)\|limitTo\:(\d+)/)?b.replace(/(.*?)\|limitTo\:(\d+)/,function(a,b,d){var b=c[b]?c[b]:"",e=b.substring(0,d);return e=b.length>e.length?e.substring(0,e.length-3)+"...":e}):c[b]?c[b]:""});case"function":return b(c);default:return b}},text2Color:function(a){if(!a||0==a.length)return!1;for(var b=0,c=0;b<a.length;c=a.charCodeAt(b++)+((c<<5)-c));for(var b=0,d="#";b<3;d+=("00"+(c>>2*b++&255).toString(16)).slice(-2));return d}},files:null,_itFl:[],_itFc:null,_itFr:[],_itPl:[],_ajFc:0};return e.on("filer.append",function(a,b){m._append(a,b)}).on("filer.remove",function(a,b){b.binded=!0,m._remove(a,b)}).on("filer.reset",function(a){return m._reset(),m._clear(),!0}).on("filer.generateList",function(a,b){return m._getList(a,b)}).on("filer.retry",function(a,b){return m._retryUpload(a,b)}),m.init(),this})},a.fn.filer.defaults={limit:null,maxSize:null,fileMaxSize:null,extensions:null,changeInput:!0,showThumbs:!1,appendTo:null,theme:"default",templates:{box:'<ul class="jFiler-items-list jFiler-items-default"></ul>',item:'<li class="jFiler-item"><div class="jFiler-item-container"><div class="jFiler-item-inner"><div class="jFiler-item-icon pull-left">{{fi-icon}}</div><div class="jFiler-item-info pull-left"><div class="jFiler-item-title" title="{{fi-name}}">{{fi-name | limitTo:30}}</div><div class="jFiler-item-others"><span>size: {{fi-size2}}</span><span>type: {{fi-extension}}</span><span class="jFiler-item-status">{{fi-progressBar}}</span></div><div class="jFiler-item-assets"><ul class="list-inline"><li><a class="icon-jfi-trash jFiler-item-trash-action"></a></li></ul></div></div></div></div></li>',itemAppend:'<li class="jFiler-item"><div class="jFiler-item-container"><div class="jFiler-item-inner"><div class="jFiler-item-icon pull-left">{{fi-icon}}</div><div class="jFiler-item-info pull-left"><div class="jFiler-item-title">{{fi-name | limitTo:35}}</div><div class="jFiler-item-others"><span>size: {{fi-size2}}</span><span>type: {{fi-extension}}</span><span class="jFiler-item-status"></span></div><div class="jFiler-item-assets"><ul class="list-inline"><li><a class="icon-jfi-trash jFiler-item-trash-action"></a></li></ul></div></div></div></div></li>',progressBar:'<div class="bar"></div>',itemAppendToEnd:!1,removeConfirmation:!0,canvasImage:!0,_selectors:{list:".jFiler-items-list",item:".jFiler-item",progressBar:".bar",remove:".jFiler-item-trash-action"}},files:null,uploadFile:null,dragDrop:null,addMore:!1,allowDuplicates:!1,clipBoardPaste:!0,excludeName:null,beforeRender:null,afterRender:null,beforeShow:null,beforeSelect:null,onSelect:null,onFileCheck:null,afterShow:null,onRemove:null,onEmpty:null,options:null,dialogs:{alert:function(a){return alert(a)},confirm:function(a,b){confirm(a)?b():null}},captions:{button:"Choose Files",feedback:"Choose files To Upload",feedback2:"files were chosen",drop:"Drop file here to Upload",removeConfirmation:"Are you sure you want to remove this file?",errors:{filesLimit:"Only {{fi-limit}} files are allowed to be uploaded.",filesType:"Only Images are allowed to be uploaded.",filesSize:"{{fi-name}} is too large! Please upload file up to {{fi-fileMaxSize}} MB.",filesSizeAll:"Files you've choosed are too large! Please upload files up to {{fi-maxSize}} MB.",folderUpload:"You are not allowed to upload folders."}}}}(jQuery);
if (!Array.prototype.find) {
    Array.prototype.find = function (predicate) {
        'use strict';
        if (this == null) {
            throw new TypeError('Array.prototype.find called on null or undefined');
        }
        if (typeof predicate !== 'function') {
            throw new TypeError('predicate must be a function');
        }
        var list = Object(this);
        var length = list.length >>> 0;
        var thisArg = arguments[1];
        var value;

        for (var i = 0; i < length; i++) {
            value = list[i];
            if (predicate.call(thisArg, value, i, list)) {
                return value;
            }
        }
        return undefined;
    };
}

if (!Date.prototype.toShortDateString) {
    Date.prototype.toShortDateString = function () {
        return this.getFullYear() +
            "-" +
            ("0" + (this.getMonth() + 1)).slice(-2) +
            "-" +
            ("0" + this.getDate()).slice(-2);
    }
}

var ImagePlaceHolder = function () {
    var elements = $("img[data-img-source]");
    elements.each(function () {
        var element = this;
        $(element).attr("src", element.dataset["imgSource"]);
        $(element).on("error",
            function (event) {
                $(event.target).on("error", null);
                if (event.target.dataset["placeholder"]) {
                    $(event.target).attr("src", event.target.dataset["placeholder"]);
                }
            });
    });
}

var UploadImageModule = function (options) {
    var settings = {
        fileSelector: "",
        previewImgSelector: "",
        uploadBtnSelector: "",
        linkSelector: "",
        handleUrl: "",
        cancelUploadBtnSelector: "",
        deleteLinkSelector: "",
        handleDeleteUrl: "",
        errorMessage: "",
        prevImageData: "",
        isNeedToUpdateHeader: false,

        messages: {
            confirmationDialogSelector: "#dialog-confirmation",
            delConfirmationMess: "Är du säker på att du vill ta bort bilden?",
            yesDelBtnText: "Ja, ta bort",
            noDelBtnText: "Nej, spara den",

            uploadingErrorMessage: "Något gick fel när du försökte ladda upp bilden. Vänligen försök igen.",
            uploadingErrorTitle: "Bilden kunde inte laddas upp",

            informationDialogSelector: "#informationDialog"
        }
        //addition for dialogs
        //confirmationDialogSelector: "#dialog-confirmation",
        //delConfirmationMess: "Är du säker på att du vill ta bort bilden?",
        //yesDelBtnText: "Ja, ta bort",
        //noDelBtnText: "Nej, spara den"
    };
    var _prevImageData;
    var _data = []; //add params to request following format [{"key": "id", "value": 1},{"key": "name", "value": "abc"}]

    $.extend(settings, options);

    var buildParamsForRequest = function () {
        var data = new FormData();
        if (_data && _data.length > 0) {
            for (var i = 0; i < _data.length; i++) {
                var param = _data[i];
                if (param.key && param.value) {
                    data.append(param.key, param.value);
                }
            }
        }
        return data;
    }

    var attachFileOnChangeEvent = function () {
        $(settings.fileSelector).change(function () {
            var file = this.files[0];
            var imagefile = file.type;
            var match = ["image/jpeg", "image/png", "image/jpg"];
            if (!((imagefile === match[0]) || (imagefile === match[1]) || (imagefile === match[2]))) {
                $(settings.previewImgSelector).attr('src', _prevImageData);
                return false;
            } else {
                var reader = new FileReader();
                reader.onload = function (event) {
                    $(settings.previewImgSelector).attr('src', event.target.result);
                    //show Cancel/Upload buttons
                    $(settings.uploadBtnSelector).parent().show();

                };
                reader.readAsDataURL(this.files[0]);
            }
        });
    };

    var attachOpenFileDialog = function () {
        $(settings.linkSelector).on('click',
            function (e) {
                $('.popup-sub, .list-action-item').hide();
                e.preventDefault();
                $(settings.fileSelector).trigger('click');
            });
    }

    var attachUploadFileClickEvent = function () {
        $(settings.uploadBtnSelector).on('click', function () {
            var data = buildParamsForRequest();
            var files = $(settings.fileSelector).get(0).files;

            // Add the uploaded image content to the form data collection
            if (files.length > 0) {
                var fileSize = files[0].size;
                if (fileSize > 4096000) {
                    showInformationDialog("", "Filen är för stor. Maximal filstorlek 4MB.");
                    return;
                }
                data.append("UploadedImage", files[0]);
            }
            $(settings.previewImgSelector).prev().show();
            $(settings.uploadBtnSelector).parent().hide();
            $.ajax({
                url: settings.handleUrl,
                type: "POST",
                data: data,
                contentType: false, // The content type used when sending data to the server.
                cache: false,
                processData: false, // To send DOMDocument or non processed data file it is set to false
                success: function (result) {
                    $(settings.previewImgSelector).prev().hide();

                    if (result.success) {
                        $(settings.uploadBtnSelector).parent().hide();
                        $(settings.deleteLinkSelector).show();

                        _prevImageData = $(settings.previewImgSelector).attr("src");
                        if (settings.isNeedToUpdateHeader) {
                            $('.lm__user-avatar-wrapper .header_user_avatar').attr('src', _prevImageData);
                        }

                        MachineDetailPage.autoFitImages();
                    } else {
                        $(settings.previewImgSelector).attr("src", _prevImageData);
                        //var message = settings.errorMessage!=="" ? settings.errorMessage : "Det uppstod ett fel när bilden skulle laddas upp. Var god försök igen!";
                        showInformationDialog(settings.messages.uploadingErrorTitle, settings.messages.uploadingErrorMessage);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $(settings.previewImgSelector).prev().hide();
                    $(settings.previewImgSelector).attr("src", _prevImageData);
                    var message = "Det uppstod ett fel när bilden skulle laddas upp: " + errorThrown.toString();
                    showInformationDialog(this.settings.uploadingErrorTitle, message);
                }
            });
        });
    }

    var attactCancelUploadEvent = function () {
        $(settings.cancelUploadBtnSelector).on('click', function () {
            //$(_fileSelector).val('');
            $(settings.fileSelector).val('');
            $(settings.uploadBtnSelector).parent().hide();
            $(settings.previewImgSelector).attr("src", _prevImageData);
        });
    }
    var deleteImage = function () {
        $(settings.previewImgSelector).prev().show();
        $('.popup-sub, .list-action-item').hide();

        var data = buildParamsForRequest();

        $.ajax({
            url: settings.handleDeleteUrl,
            type: "POST",
            data: data,
            contentType: false, // The content type used when sending data to the server.
            cache: false,
            processData: false, // To send DOMDocument or non processed data file it is set to false
            success: function (result) {
                $(settings.previewImgSelector).prev().hide();
                if (result.success) {
                    _prevImageData = result.imageUrl;

                    $(settings.deleteLinkSelector).hide();
                    $(settings.previewImgSelector).attr("src", _prevImageData);

                    if (this.settings.isNeedToUpdateHeader) { //only using for update user avatar on header
                        $('.lm__user-avatar-wrapper .header_user_avatar').attr('src', _prevImageData);
                    }
                } else {
                    $(settings.previewImgSelector).attr("src", _prevImageData);
                    //alert("Det uppstod ett fel när bilden skulle laddas upp. Var god försök igen!");
                    showInformationDialog("", "Det uppstod ett fel när bilden skulle laddas upp. Var god försök igen!");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(settings.previewImgSelector).prev().hide();
                $(settings.previewImgSelector).attr("src", _prevImageData);
                //alert("Det uppstod ett fel när bilden skulle tas bort: " + errorThrown.toString());
                var message = "Det uppstod ett fel när bilden skulle tas bort: " + errorThrown.toString();
                showInformationDialog(message);
            }
        });
    }

    var attachDeleteFileClickEvent = function () {
        $(settings.deleteLinkSelector).on('click', function () {
            showDelConfirmationDialog(deleteImage);
        });
    }


    function showDelConfirmationDialog(callback) {
        var $dialog = $(settings.messages.confirmationDialogSelector);
        if ($dialog.length === 0) {
            return;
        }
        $(".success-header-title", $dialog).html(settings.delConfirmationMess);
        var $yesBtn = $(".success-confirm-inform.yes", $dialog);
        var $noBtn = $(".success-confirm-inform.no", $dialog);
        $noBtn.html(settings.noDelBtnText);
        $yesBtn.html(settings.yesDelBtnText);

        $yesBtn.click(function () {
            callback();
        });

        $dialog.fadeIn();
    }
    var initDataForRequest = function (data) {
        _data = data;
    }

    var initUploadFile = function (options) {
        if (!options) {
            $.extend(settings, options);
        }
        _prevImageData = $(settings.previewImgSelector).attr("src");

        attachOpenFileDialog();
        attachFileOnChangeEvent();
        attachUploadFileClickEvent();
        attactCancelUploadEvent();
    };

    var initDeleteFile = function (options) {
        if (!options) {
            $.extend(settings, options);
        }

        attachDeleteFileClickEvent();
    }

    function showInformationDialog(title, message) {
        var $informationDialog = $(settings.messages.informationDialogSelector);
        $informationDialog.find('#dialogTitle').html(title);
        $informationDialog.find('#dialogContent').html(message);
        if ($informationDialog.hasClass('hidden')) {
            $informationDialog.removeClass('hidden');
        } else {
            $informationDialog.fadeIn();
        }
    }

    return {
        initDataForRequest: initDataForRequest,
        initUploadFile: initUploadFile,
        initDeleteFile: initDeleteFile
    };
};

var GroCommon = GroCommon ||
(function () {

    var dateFromISO = function (s) {
        s = s.split(/\D/);
        return new Date(Date.UTC(s[0], --s[1] || '', s[2] || '', s[3] || '', s[4] || '', s[5] || '', s[6] || ''));
    };

    var parseInt32 = function (s) {
        var value = parseInt(s);
        if (isNaN(value)) value = 0;

        return value;
    }

    var blockUI = function () {
        //Block UI
    };

    var unblockUI = function () {
        //Unblock UI
    };
    var handleExternalLink = function () {
        var $a = $('a');
        $a.each(function () {
            var href = $(this).attr('href');
            var target = $(this).attr("target");

            if (!!target) {
                return;
            }

            var isExternalLink = href != undefined && href.trim().indexOf("/") !== 0 && href.trim() !== "#" && href.trim().indexOf("javascript:void") && href.trim().indexOf("javascript:window.print()") < 0;
            if (isExternalLink) {
                $(this).attr("target", "_blank");
            }
        });
    }

    var getIdxOfElement = function ($element, $array) {
        return $array.index($element);
    };

    var disableOrEnableElement = function (elementSelector, $currentInput) {
        var agreementElements = $(elementSelector);
        var $currentElement = $currentInput.closest(elementSelector);
        if (agreementElements.length > 0) {
            var idx = getIdxOfElement($currentElement, $(agreementElements));
            if (idx >= 0 && idx < agreementElements.length - 1) {
                // enable the element right after the current element.
                var $nextElement = $(agreementElements[idx + 1]);
                if ($nextElement.hasClass('disabled')) {
                    $nextElement.removeClass('disabled');
                }
            }
        }
    }

    return {
        blockUI: blockUI,
        unblockUI: unblockUI,
        handleExternalLink: handleExternalLink,
        dateFromISO: dateFromISO,
        parseInt: parseInt32,
        disableOrEnableElement: disableOrEnableElement
    };
})();

$(function () {
    ImagePlaceHolder();
});

$(function () {
    $("input[type=button].submit").click(function() {
        var form = $(event.target).closest("form");
        if (form) {
            $(form).submit();
        }
    });

    $("form").keyup(function (event) {
        if (event.which === 13) {
            // key enter
            var form = $(event.target).closest("form");
            if (form) {
                var submitBtn = getSubmitBtn(form);
                if (submitBtn !== undefined) {
                    $(submitBtn).click();
                }
            }
        }
    });
    function getSubmitBtn(form) {
        var btnSubmit = $("button.submit.trigger-on-enter", $(form));
        if (btnSubmit.length > 0) {
            return btnSubmit;
        }

        var btnTypeSubmit = $("button[type=submit].trigger-on-enter", $(form));
        if (btnTypeSubmit.length > 0) {
            return btnTypeSubmit;
        }

        var inputSubmit = $("input[type=submit].trigger-on-enter", $(form));
        if (inputSubmit.length > 0) {
            return inputSubmit;
        }
        return undefined;
    }

});
/*! Tablesaw - v2.0.3 - 2016-06-03
* https://github.com/filamentgroup/tablesaw
* Copyright (c) 2016 Filament Group; Licensed MIT */
/*
* tablesaw: A set of plugins for responsive tables
* Stack and Column Toggle tables
* Copyright (c) 2013 Filament Group, Inc.
* MIT License
*/

if( typeof Tablesaw === "undefined" ) {
	Tablesaw = {
		i18n: {
			modes: [ 'Stack', 'Swipe', 'Toggle' ],
			columns: 'Col<span class=\"a11y-sm\">umn</span>s',
			columnBtnText: 'Columns',
			columnsDialogError: 'No eligible columns.',
			sort: 'Sort'
		},
		// cut the mustard
		mustard: 'querySelector' in document &&
			( !window.blackberry || window.WebKitPoint ) &&
			!window.operamini
	};
}
if( !Tablesaw.config ) {
	Tablesaw.config = {};
}
if( Tablesaw.mustard ) {
	jQuery( document.documentElement ).addClass( 'tablesaw-enhanced' );
}

;(function( $ ) {
	var pluginName = "table",
		classes = {
			toolbar: "tablesaw-bar"
		},
		events = {
			create: "tablesawcreate",
			destroy: "tablesawdestroy",
			refresh: "tablesawrefresh"
		},
		defaultMode = "stack",
		initSelector = "table[data-tablesaw-mode],table[data-tablesaw-sortable]";

	var Table = function( element ) {
		if( !element ) {
			throw new Error( "Tablesaw requires an element." );
		}

		this.table = element;
		this.$table = $( element );

		this.mode = this.$table.attr( "data-tablesaw-mode" ) || defaultMode;

		this.init();
	};

	Table.prototype.init = function() {
		// assign an id if there is none
		if ( !this.$table.attr( "id" ) ) {
			this.$table.attr( "id", pluginName + "-" + Math.round( Math.random() * 10000 ) );
		}

		this.createToolbar();

		var colstart = this._initCells();

		this.$table.trigger( events.create, [ this, colstart ] );
	};

	Table.prototype._initCells = function() {
		var colstart,
			thrs = this.table.querySelectorAll( "thead tr" ),
			self = this;

		$( thrs ).each( function(){
			var coltally = 0;

			$( this ).children().each( function(){
				var span = parseInt( this.getAttribute( "colspan" ), 10 ),
					sel = ":nth-child(" + ( coltally + 1 ) + ")";

				colstart = coltally + 1;

				if( span ){
					for( var k = 0; k < span - 1; k++ ){
						coltally++;
						sel += ", :nth-child(" + ( coltally + 1 ) + ")";
					}
				}

				// Store "cells" data on header as a reference to all cells in the same column as this TH
				this.cells = self.$table.find("tr").not( thrs[0] ).not( this ).children().filter( sel );
				coltally++;
			});
		});

		return colstart;
	};

	Table.prototype.refresh = function() {
		this._initCells();

		this.$table.trigger( events.refresh );
	};

	Table.prototype.createToolbar = function() {
		// Insert the toolbar
		// TODO move this into a separate component
		var $toolbar = this.$table.prev().filter( '.' + classes.toolbar );
		if( !$toolbar.length ) {
			$toolbar = $( '<div>' )
				.addClass( classes.toolbar )
				.insertBefore( this.$table );
		}
		this.$toolbar = $toolbar;

		if( this.mode ) {
			this.$toolbar.addClass( 'mode-' + this.mode );
		}
	};

	Table.prototype.destroy = function() {
		// Don’t remove the toolbar. Some of the table features are not yet destroy-friendly.
		this.$table.prev().filter( '.' + classes.toolbar ).each(function() {
			this.className = this.className.replace( /\bmode\-\w*\b/gi, '' );
		});

		var tableId = this.$table.attr( 'id' );
		$( document ).unbind( "." + tableId );
		$( window ).unbind( "." + tableId );

		// other plugins
		this.$table.trigger( events.destroy, [ this ] );

		this.$table.removeData( pluginName );
	};

	// Collection method.
	$.fn[ pluginName ] = function() {
		return this.each( function() {
			var $t = $( this );

			if( $t.data( pluginName ) ){
				return;
			}

			var table = new Table( this );
			$t.data( pluginName, table );
		});
	};

	$( document ).on( "enhance.tablesaw", function( e ) {
		// Cut the mustard
		if( Tablesaw.mustard ) {
			$( e.target ).find( initSelector )[ pluginName ]();
		}
	});

}( jQuery ));

;(function( win, $, undefined ){

	var classes = {
		stackTable: 'tablesaw-stack',
		cellLabels: 'tablesaw-cell-label',
		cellContentLabels: 'tablesaw-cell-content'
	};

	var data = {
		obj: 'tablesaw-stack'
	};

	var attrs = {
		labelless: 'data-tablesaw-no-labels',
		hideempty: 'data-tablesaw-hide-empty'
	};

	var Stack = function( element ) {

		this.$table = $( element );

		this.labelless = this.$table.is( '[' + attrs.labelless + ']' );
		this.hideempty = this.$table.is( '[' + attrs.hideempty + ']' );

		if( !this.labelless ) {
			// allHeaders references headers, plus all THs in the thead, which may include several rows, or not
			this.allHeaders = this.$table.find( "th" );
		}

		this.$table.data( data.obj, this );
	};

	Stack.prototype.init = function( colstart ) {
		this.$table.addClass( classes.stackTable );

		if( this.labelless ) {
			return;
		}

		// get headers in reverse order so that top-level headers are appended last
		var reverseHeaders = $( this.allHeaders );
		var hideempty = this.hideempty;
		
		// create the hide/show toggles
		reverseHeaders.each(function(){
			var $t = $( this ),
				$cells = $( this.cells ).filter(function() {
					return !$( this ).parent().is( "[" + attrs.labelless + "]" ) && ( !hideempty || !$( this ).is( ":empty" ) );
				}),
				hierarchyClass = $cells.not( this ).filter( "thead th" ).length && " tablesaw-cell-label-top",
				// TODO reduce coupling with sortable
				$sortableButton = $t.find( ".tablesaw-sortable-btn" ),
				html = $sortableButton.length ? $sortableButton.html() : $t.html();

			if( html !== "" ){
				if( hierarchyClass ){
					var iteration = parseInt( $( this ).attr( "colspan" ), 10 ),
						filter = "";

					if( iteration ){
						filter = "td:nth-child("+ iteration +"n + " + ( colstart ) +")";
					}
					$cells.filter( filter ).prepend( "<b class='" + classes.cellLabels + hierarchyClass + "'>" + html + "</b>"  );
				} else {
					$cells.wrapInner( "<span class='" + classes.cellContentLabels + "'></span>" );
					$cells.prepend( "<b class='" + classes.cellLabels + "'>" + html + "</b>"  );
				}
			}
		});
	};

	Stack.prototype.destroy = function() {
		this.$table.removeClass( classes.stackTable );
		this.$table.find( '.' + classes.cellLabels ).remove();
		this.$table.find( '.' + classes.cellContentLabels ).each(function() {
			$( this ).replaceWith( this.childNodes );
		});
	};

	// on tablecreate, init
	$( document ).on( "tablesawcreate", function( e, Tablesaw, colstart ){
		if( Tablesaw.mode === 'stack' ){
			var table = new Stack( Tablesaw.table );
			table.init( colstart );
		}

	} );

	$( document ).on( "tablesawdestroy", function( e, Tablesaw ){

		if( Tablesaw.mode === 'stack' ){
			$( Tablesaw.table ).data( data.obj ).destroy();
		}

	} );

}( this, jQuery ));
;(function( $ ) {
	var pluginName = "tablesawbtn",
		methods = {
			_create: function(){
				return $( this ).each(function() {
					$( this )
						.trigger( "beforecreate." + pluginName )
						[ pluginName ]( "_init" )
						.trigger( "create." + pluginName );
				});
			},
			_init: function(){
				var oEl = $( this ),
					sel = this.getElementsByTagName( "select" )[ 0 ];

				if( sel ) {
					$( this )
						.addClass( "btn-select" )
						[ pluginName ]( "_select", sel );
				}
				return oEl;
			},
			_select: function( sel ) {
				var update = function( oEl, sel ) {
					var opts = $( sel ).find( "option" ),
						label, el, children;

					opts.each(function() {
						var opt = this;
						if( opt.selected ) {
							label = document.createTextNode( opt.text );
						}
					});

					children = oEl.childNodes;
					if( opts.length > 0 ){
						for( var i = 0, l = children.length; i < l; i++ ) {
							el = children[ i ];

							if( el && el.nodeType === 3 ) {
								oEl.replaceChild( label, el );
							}
						}
					}
				};

				update( this, sel );
				$( this ).bind( "change refresh", function() {
					update( this, sel );
				});
			}
		};

	// Collection method.
	$.fn[ pluginName ] = function( arrg, a, b, c ) {
		return this.each(function() {

		// if it's a method
		if( arrg && typeof( arrg ) === "string" ){
			return $.fn[ pluginName ].prototype[ arrg ].call( this, a, b, c );
		}

		// don't re-init
		if( $( this ).data( pluginName + "active" ) ){
			return $( this );
		}

		// otherwise, init

		$( this ).data( pluginName + "active", true );
			$.fn[ pluginName ].prototype._create.call( this );
		});
	};

	// add methods
	$.extend( $.fn[ pluginName ].prototype, methods );

}( jQuery ));
;(function( win, $, undefined ){

	var ColumnToggle = function( element ) {

		this.$table = $( element );

		this.classes = {
			columnToggleTable: 'tablesaw-columntoggle',
			columnBtnContain: 'tablesaw-columntoggle-btnwrap tablesaw-advance',
			columnBtn: 'tablesaw-columntoggle-btn tablesaw-nav-btn down',
			popup: 'tablesaw-columntoggle-popup',
			priorityPrefix: 'tablesaw-priority-',
			// TODO duplicate class, also in tables.js
			toolbar: 'tablesaw-bar'
		};

		// Expose headers and allHeaders properties on the widget
		// headers references the THs within the first TR in the table
		this.headers = this.$table.find( 'tr:first > th' );

		this.$table.data( 'tablesaw-coltoggle', this );
	};

	ColumnToggle.prototype.init = function() {

		var tableId,
			id,
			$menuButton,
			$popup,
			$menu,
			$btnContain,
			self = this;

		this.$table.addClass( this.classes.columnToggleTable );

		tableId = this.$table.attr( "id" );
		id = tableId + "-popup";
		$btnContain = $( "<div class='" + this.classes.columnBtnContain + "'></div>" );
		$menuButton = $( "<a href='#" + id + "' class='btn btn-micro " + this.classes.columnBtn +"' data-popup-link>" +
										"<span>" + Tablesaw.i18n.columnBtnText + "</span></a>" );
		$popup = $( "<div class='dialog-table-coltoggle " + this.classes.popup + "' id='" + id + "'></div>" );
		$menu = $( "<div class='btn-group'></div>" );

		var hasNonPersistentHeaders = false;
		$( this.headers ).not( "td" ).each( function() {
			var $this = $( this ),
				priority = $this.attr("data-tablesaw-priority"),
				$cells = self.$getCells( this );

			if( priority && priority !== "persist" ) {
				$cells.addClass( self.classes.priorityPrefix + priority );

				$("<label><input type='checkbox' checked>" + $this.text() + "</label>" )
					.appendTo( $menu )
					.children( 0 )
					.data( "tablesaw-header", this );

				hasNonPersistentHeaders = true;
			}
		});

		if( !hasNonPersistentHeaders ) {
			$menu.append( '<label>' + Tablesaw.i18n.columnsDialogError + '</label>' );
		}

		$menu.appendTo( $popup );

		// bind change event listeners to inputs - TODO: move to a private method?
		$menu.find( 'input[type="checkbox"]' ).on( "change", function(e) {
			var checked = e.target.checked;

			self.$getCellsFromCheckbox( e.target )
				.toggleClass( "tablesaw-cell-hidden", !checked )
				.toggleClass( "tablesaw-cell-visible", checked );

			self.$table.trigger( 'tablesawcolumns' );
		});

		$menuButton.appendTo( $btnContain );
		$btnContain.appendTo( this.$table.prev().filter( '.' + this.classes.toolbar ) );


		function closePopup( event ) {
			// Click came from inside the popup, ignore.
			if( event && $( event.target ).closest( "." + self.classes.popup ).length ) {
				return;
			}

			$( document ).unbind( 'click.' + tableId );
			$menuButton.removeClass( 'up' ).addClass( 'down' );
			$btnContain.removeClass( 'visible' );
		}

		var closeTimeout;
		function openPopup() {
			$btnContain.addClass( 'visible' );
			$menuButton.removeClass( 'down' ).addClass( 'up' );

			$( document ).unbind( 'click.' + tableId, closePopup );

			window.clearTimeout( closeTimeout );
			closeTimeout = window.setTimeout(function() {
				$( document ).one( 'click.' + tableId, closePopup );
			}, 15 );
		}

		$menuButton.on( "click.tablesaw", function( event ) {
			event.preventDefault();

			if( !$btnContain.is( ".visible" ) ) {
				openPopup();
			} else {
				closePopup();
			}
		});

		$popup.appendTo( $btnContain );

		this.$menu = $menu;

		$(window).on( "resize." + tableId, function(){
			self.refreshToggle();
		});

		this.refreshToggle();
	};

	ColumnToggle.prototype.$getCells = function( th ) {
		return $( th ).add( th.cells );
	};

	ColumnToggle.prototype.$getCellsFromCheckbox = function( checkbox ) {
		var th = $( checkbox ).data( "tablesaw-header" );
		return this.$getCells( th );
	};

	ColumnToggle.prototype.refreshToggle = function() {
		var self = this;
		this.$menu.find( "input" ).each( function() {
			this.checked = self.$getCellsFromCheckbox( this ).eq( 0 ).css( "display" ) === "table-cell";
		});
	};

	ColumnToggle.prototype.refreshPriority = function(){
		var self = this;
		$(this.headers).not( "td" ).each( function() {
			var $this = $( this ),
				priority = $this.attr("data-tablesaw-priority"),
				$cells = $this.add( this.cells );

			if( priority && priority !== "persist" ) {
				$cells.addClass( self.classes.priorityPrefix + priority );
			}
		});
	};

	ColumnToggle.prototype.destroy = function() {
		// table toolbars, document and window .tableId events
		// removed in parent tables.js destroy method

		this.$table.removeClass( this.classes.columnToggleTable );
		this.$table.find( 'th, td' ).each(function() {
			var $cell = $( this );
			$cell.removeClass( 'tablesaw-cell-hidden' )
				.removeClass( 'tablesaw-cell-visible' );

			this.className = this.className.replace( /\bui\-table\-priority\-\d\b/g, '' );
		});
	};

	// on tablecreate, init
	$( document ).on( "tablesawcreate", function( e, Tablesaw ){

		if( Tablesaw.mode === 'columntoggle' ){
			var table = new ColumnToggle( Tablesaw.table );
			table.init();
		}

	} );

	$( document ).on( "tablesawdestroy", function( e, Tablesaw ){
		if( Tablesaw.mode === 'columntoggle' ){
			$( Tablesaw.table ).data( 'tablesaw-coltoggle' ).destroy();
		}
	} );

}( this, jQuery ));
;(function( win, $, undefined ){

	$.extend( Tablesaw.config, {
		swipe: {
			horizontalThreshold: 15,
			verticalThreshold: 30
		}
	});

	function isIE8() {
		var div = document.createElement('div'),
			all = div.getElementsByTagName('i');

		div.innerHTML = '<!--[if lte IE 8]><i></i><![endif]-->';

		return !!all.length;
	}

	var classes = {
		// TODO duplicate class, also in tables.js
		toolbar: "tablesaw-bar",
		hideBtn: "disabled",
		persistWidths: "tablesaw-fix-persist",
		allColumnsVisible: 'tablesaw-all-cols-visible'
	};

	function createSwipeTable( $table ){

		var $btns = $( "<div class='tablesaw-advance'></div>" ),
			$prevBtn = $( "<a href='#' class='tablesaw-nav-btn btn btn-micro left' title='Previous Column'></a>" ).appendTo( $btns ),
			$nextBtn = $( "<a href='#' class='tablesaw-nav-btn btn btn-micro right' title='Next Column'></a>" ).appendTo( $btns ),
			$headerCells = $table.find( "thead th" ),
			$headerCellsNoPersist = $headerCells.not( '[data-tablesaw-priority="persist"]' ),
			headerWidths = [],
			$head = $( document.head || 'head' ),
			tableId = $table.attr( 'id' ),
			// TODO switch this to an nth-child feature test
			supportsNthChild = !isIE8();

		if( !$headerCells.length ) {
			throw new Error( "tablesaw swipe: no header cells found. Are you using <th> inside of <thead>?" );
		}

		// Calculate initial widths
		$table.css('width', 'auto');
		$headerCells.each(function() {
			headerWidths.push( $( this ).outerWidth() );
		});
		$table.css( 'width', '' );

		$btns.appendTo( $table.prev().filter( '.tablesaw-bar' ) );

		$table.addClass( "tablesaw-swipe" );

		if( !tableId ) {
			tableId = 'tableswipe-' + Math.round( Math.random() * 10000 );
			$table.attr( 'id', tableId );
		}

		function $getCells( headerCell ) {
			return $( headerCell.cells ).add( headerCell );
		}

		function showColumn( headerCell ) {
			$getCells( headerCell ).removeClass( 'tablesaw-cell-hidden' );
		}

		function hideColumn( headerCell ) {
			$getCells( headerCell ).addClass( 'tablesaw-cell-hidden' );
		}

		function persistColumn( headerCell ) {
			$getCells( headerCell ).addClass( 'tablesaw-cell-persist' );
		}

		function isPersistent( headerCell ) {
			return $( headerCell ).is( '[data-tablesaw-priority="persist"]' );
		}

		function unmaintainWidths() {
			$table.removeClass( classes.persistWidths );
			$( '#' + tableId + '-persist' ).remove();
		}

		function maintainWidths() {
			var prefix = '#' + tableId + '.tablesaw-swipe ',
				styles = [],
				tableWidth = $table.width(),
				hash = [],
				newHash;

			$headerCells.each(function( index ) {
				var width;
				if( isPersistent( this ) ) {
					width = $( this ).outerWidth();

					// Only save width on non-greedy columns (take up less than 75% of table width)
					if( width < tableWidth * 0.75 ) {
						hash.push( index + '-' + width );
						styles.push( prefix + ' .tablesaw-cell-persist:nth-child(' + ( index + 1 ) + ') { width: ' + width + 'px; }' );
					}
				}
			});
			newHash = hash.join( '_' );

			$table.addClass( classes.persistWidths );

			var $style = $( '#' + tableId + '-persist' );
			// If style element not yet added OR if the widths have changed
			if( !$style.length || $style.data( 'hash' ) !== newHash ) {
				// Remove existing
				$style.remove();

				if( styles.length ) {
					$( '<style>' + styles.join( "\n" ) + '</style>' )
						.attr( 'id', tableId + '-persist' )
						.data( 'hash', newHash )
						.appendTo( $head );
				}
			}
		}

		function getNext(){
			var next = [],
				checkFound;

			$headerCellsNoPersist.each(function( i ) {
				var $t = $( this ),
					isHidden = $t.css( "display" ) === "none" || $t.is( ".tablesaw-cell-hidden" );

				if( !isHidden && !checkFound ) {
					checkFound = true;
					next[ 0 ] = i;
				} else if( isHidden && checkFound ) {
					next[ 1 ] = i;

					return false;
				}
			});

			return next;
		}

		function getPrev(){
			var next = getNext();
			return [ next[ 1 ] - 1 , next[ 0 ] - 1 ];
		}

		function nextpair( fwd ){
			return fwd ? getNext() : getPrev();
		}

		function canAdvance( pair ){
			return pair[ 1 ] > -1 && pair[ 1 ] < $headerCellsNoPersist.length;
		}

		function matchesMedia() {
			var matchMedia = $table.attr( "data-tablesaw-swipe-media" );
			return !matchMedia || ( "matchMedia" in win ) && win.matchMedia( matchMedia ).matches;
		}

		function fakeBreakpoints() {
			if( !matchesMedia() ) {
				return;
			}

			var extraPaddingPixels = 20,
				containerWidth = $table.parent().width(),
				persist = [],
				sum = 0,
				sums = [],
				visibleNonPersistantCount = $headerCells.length;

			$headerCells.each(function( index ) {
				var $t = $( this ),
					isPersist = $t.is( '[data-tablesaw-priority="persist"]' );

				persist.push( isPersist );

				sum += headerWidths[ index ] + ( isPersist ? 0 : extraPaddingPixels );
				sums.push( sum );

				// is persistent or is hidden
				if( isPersist || sum > containerWidth ) {
					visibleNonPersistantCount--;
				}
			});

			var needsNonPersistentColumn = visibleNonPersistantCount === 0;

			$headerCells.each(function( index ) {
				if( persist[ index ] ) {

					// for visual box-shadow
					persistColumn( this );
					return;
				}

				if( sums[ index ] <= containerWidth || needsNonPersistentColumn ) {
					needsNonPersistentColumn = false;
					showColumn( this );
				} else {
					hideColumn( this );
				}
			});

			if( supportsNthChild ) {
				unmaintainWidths();
			}
			$table.trigger( 'tablesawcolumns' );
		}

		function advance( fwd ){
			var pair = nextpair( fwd );
			if( canAdvance( pair ) ){
				if( isNaN( pair[ 0 ] ) ){
					if( fwd ){
						pair[0] = 0;
					}
					else {
						pair[0] = $headerCellsNoPersist.length - 1;
					}
				}

				if( supportsNthChild ) {
					maintainWidths();
				}

				hideColumn( $headerCellsNoPersist.get( pair[ 0 ] ) );
				showColumn( $headerCellsNoPersist.get( pair[ 1 ] ) );

				$table.trigger( 'tablesawcolumns' );
			}
		}

		$prevBtn.add( $nextBtn ).click(function( e ){
			advance( !!$( e.target ).closest( $nextBtn ).length );
			e.preventDefault();
		});

		function getCoord( event, key ) {
			return ( event.touches || event.originalEvent.touches )[ 0 ][ key ];
		}

		$table
			.bind( "touchstart.swipetoggle", function( e ){
				var originX = getCoord( e, 'pageX' ),
					originY = getCoord( e, 'pageY' ),
					x,
					y;

				$( win ).off( "resize", fakeBreakpoints );

				$( this )
					.bind( "touchmove", function( e ){
						x = getCoord( e, 'pageX' );
						y = getCoord( e, 'pageY' );
						var cfg = Tablesaw.config.swipe;
						if( Math.abs( x - originX ) > cfg.horizontalThreshold && Math.abs( y - originY ) < cfg.verticalThreshold ) {
							e.preventDefault();
						}
					})
					.bind( "touchend.swipetoggle", function(){
						var cfg = Tablesaw.config.swipe;
						if( Math.abs( y - originY ) < cfg.verticalThreshold ) {
							if( x - originX < -1 * cfg.horizontalThreshold ){
								advance( true );
							}
							if( x - originX > cfg.horizontalThreshold ){
								advance( false );
							}
						}

						window.setTimeout(function() {
							$( win ).on( "resize", fakeBreakpoints );
						}, 300);
						$( this ).unbind( "touchmove touchend" );
					});

			})
			.bind( "tablesawcolumns.swipetoggle", function(){
				var canGoPrev = canAdvance( getPrev() );
				var canGoNext = canAdvance( getNext() );
				$prevBtn[ canGoPrev ? "removeClass" : "addClass" ]( classes.hideBtn );
				$nextBtn[ canGoNext ? "removeClass" : "addClass" ]( classes.hideBtn );

				$prevBtn.closest( "." + classes.toolbar )[ !canGoPrev && !canGoNext ? 'addClass' : 'removeClass' ]( classes.allColumnsVisible );
			})
			.bind( "tablesawnext.swipetoggle", function(){
				advance( true );
			} )
			.bind( "tablesawprev.swipetoggle", function(){
				advance( false );
			} )
			.bind( "tablesawdestroy.swipetoggle", function(){
				var $t = $( this );

				$t.removeClass( 'tablesaw-swipe' );
				$t.prev().filter( '.tablesaw-bar' ).find( '.tablesaw-advance' ).remove();
				$( win ).off( "resize", fakeBreakpoints );

				$t.unbind( ".swipetoggle" );
			});

		fakeBreakpoints();
		$( win ).on( "resize", fakeBreakpoints );
	}



	// on tablecreate, init
	$( document ).on( "tablesawcreate", function( e, Tablesaw ){

		if( Tablesaw.mode === 'swipe' ){
			createSwipeTable( Tablesaw.$table );
		}

	} );

}( this, jQuery ));

;(function( $ ) {
	function getSortValue( cell ) {
		return $.map( cell.childNodes, function( el ) {
				var $el = $( el );
				if( $el.is( 'input, select' ) ) {
					return $el.val();
				} else if( $el.hasClass( 'tablesaw-cell-label' ) ) {
					return;
				}
				return $.trim( $el.text() );
			}).join( '' );
	}

	var pluginName = "tablesaw-sortable",
		initSelector = "table[data-" + pluginName + "]",
		sortableSwitchSelector = "[data-" + pluginName + "-switch]",
		attrs = {
			defaultCol: "data-tablesaw-sortable-default-col",
			numericCol: "data-tablesaw-sortable-numeric"
		},
		classes = {
			head: pluginName + "-head",
			ascend: pluginName + "-ascending",
			descend: pluginName + "-descending",
			switcher: pluginName + "-switch",
			tableToolbar: 'tablesaw-toolbar',
			sortButton: pluginName + "-btn"
		},
		methods = {
			_create: function( o ){
				return $( this ).each(function() {
					var init = $( this ).data( "init" + pluginName );
					if( init ) {
						return false;
					}
					$( this )
						.data( "init"+ pluginName, true )
						.trigger( "beforecreate." + pluginName )
						[ pluginName ]( "_init" , o )
						.trigger( "create." + pluginName );
				});
			},
			_init: function(){
				var el = $( this ),
					heads,
					$switcher;

				var addClassToTable = function(){
						el.addClass( pluginName );
					},
					addClassToHeads = function( h ){
						$.each( h , function( i , v ){
							$( v ).addClass( classes.head );
						});
					},
					makeHeadsActionable = function( h , fn ){
						$.each( h , function( i , v ){
							var b = $( "<button class='" + classes.sortButton + "'/>" );
							b.bind( "click" , { col: v } , fn );
							$( v ).wrapInner( b );
						});
					},
					clearOthers = function( sibs ){
						$.each( sibs , function( i , v ){
							var col = $( v );
							col.removeAttr( attrs.defaultCol );
							col.removeClass( classes.ascend );
							col.removeClass( classes.descend );
						});
					},
					headsOnAction = function( e ){
						if( $( e.target ).is( 'a[href]' ) ) {
							return;
						}

						e.stopPropagation();
						var head = $( this ).parent(),
							v = e.data.col,
							newSortValue = heads.index( head );

						clearOthers( head.siblings() );
						if( head.hasClass( classes.descend ) ){
							el[ pluginName ]( "sortBy" , v , true);
							newSortValue += '_asc';
						} else {
							el[ pluginName ]( "sortBy" , v );
							newSortValue += '_desc';
						}
						if( $switcher ) {
							$switcher.find( 'select' ).val( newSortValue ).trigger( 'refresh' );
						}

						e.preventDefault();
					},
					handleDefault = function( heads ){
						$.each( heads , function( idx , el ){
							var $el = $( el );
							if( $el.is( "[" + attrs.defaultCol + "]" ) ){
								if( !$el.hasClass( classes.descend ) ) {
									$el.addClass( classes.ascend );
								}
							}
						});
					},
					addSwitcher = function( heads ){
						$switcher = $( '<div>' ).addClass( classes.switcher ).addClass( classes.tableToolbar ).html(function() {
							var html = [ '<label>' + Tablesaw.i18n.sort + ':' ];

							html.push( '<span class="btn btn-small">&#160;<select>' );
							heads.each(function( j ) {
								var $t = $( this );
								var isDefaultCol = $t.is( "[" + attrs.defaultCol + "]" );
								var isDescending = $t.hasClass( classes.descend );

								var hasNumericAttribute = $t.is( '[data-sortable-numeric]' );
								var numericCount = 0;
								// Check only the first four rows to see if the column is numbers.
								var numericCountMax = 5;

								$( this.cells ).slice( 0, numericCountMax ).each(function() {
									if( !isNaN( parseInt( getSortValue( this ), 10 ) ) ) {
										numericCount++;
									}
								});
								var isNumeric = numericCount === numericCountMax;
								if( !hasNumericAttribute ) {
									$t.attr( "data-sortable-numeric", isNumeric ? "" : "false" );
								}

								html.push( '<option' + ( isDefaultCol && !isDescending ? ' selected' : '' ) + ' value="' + j + '_asc">' + $t.text() + ' ' + ( isNumeric ? '&#x2191;' : '(A-Z)' ) + '</option>' );
								html.push( '<option' + ( isDefaultCol && isDescending ? ' selected' : '' ) + ' value="' + j + '_desc">' + $t.text() + ' ' + ( isNumeric ? '&#x2193;' : '(Z-A)' ) + '</option>' );
							});
							html.push( '</select></span></label>' );

							return html.join('');
						});

						var $toolbar = el.prev().filter( '.tablesaw-bar' ),
							$firstChild = $toolbar.children().eq( 0 );

						if( $firstChild.length ) {
							$switcher.insertBefore( $firstChild );
						} else {
							$switcher.appendTo( $toolbar );
						}
						$switcher.find( '.btn' ).tablesawbtn();
						$switcher.find( 'select' ).on( 'change', function() {
							var val = $( this ).val().split( '_' ),
								head = heads.eq( val[ 0 ] );

							clearOthers( head.siblings() );
							el[ pluginName ]( 'sortBy', head.get( 0 ), val[ 1 ] === 'asc' );
						});
					};

					addClassToTable();
					heads = el.find( "thead th[data-" + pluginName + "-col]" );
					addClassToHeads( heads );
					makeHeadsActionable( heads , headsOnAction );
					handleDefault( heads );

					if( el.is( sortableSwitchSelector ) ) {
						addSwitcher( heads, el.find('tbody tr:nth-child(-n+3)') );
					}
			},
			getColumnNumber: function( col ){
				return $( col ).prevAll().length;
			},
			getTableRows: function(){
				return $( this ).find( "tbody tr" );
			},
			sortRows: function( rows , colNum , ascending, col ){
				var cells, fn, sorted;
				var getCells = function( rows ){
						var cells = [];
						$.each( rows , function( i , r ){
							var element = $( r ).children().get( colNum );
							cells.push({
								element: element,
								cell: getSortValue( element ),
								rowNum: i
							});
						});
						return cells;
					},
					getSortFxn = function( ascending, forceNumeric ){
						var fn,
							regex = /[^\-\+\d\.]/g;
						if( ascending ){
							fn = function( a , b ){
								if( forceNumeric ) {
									return parseFloat( a.cell.replace( regex, '' ) ) - parseFloat( b.cell.replace( regex, '' ) );
								} else {
									return a.cell.toLowerCase() > b.cell.toLowerCase() ? 1 : -1;
								}
							};
						} else {
							fn = function( a , b ){
								if( forceNumeric ) {
									return parseFloat( b.cell.replace( regex, '' ) ) - parseFloat( a.cell.replace( regex, '' ) );
								} else {
									return a.cell.toLowerCase() < b.cell.toLowerCase() ? 1 : -1;
								}
							};
						}
						return fn;
					},
					applyToRows = function( sorted , rows ){
						var newRows = [], i, l, cur;
						for( i = 0, l = sorted.length ; i < l ; i++ ){
							cur = sorted[ i ].rowNum;
							newRows.push( rows[cur] );
						}
						return newRows;
					};

				cells = getCells( rows );
				var customFn = $( col ).data( 'tablesaw-sort' );
				fn = ( customFn && typeof customFn === "function" ? customFn( ascending ) : false ) ||
					getSortFxn( ascending, $( col ).is( '[data-sortable-numeric]' ) && !$( col ).is( '[data-sortable-numeric="false"]' ) );
				sorted = cells.sort( fn );
				rows = applyToRows( sorted , rows );
				return rows;
			},
			replaceTableRows: function( rows ){
				var el = $( this ),
					body = el.find( "tbody" );
				body.html( rows );
			},
			makeColDefault: function( col , a ){
				var c = $( col );
				c.attr( attrs.defaultCol , "true" );
				if( a ){
					c.removeClass( classes.descend );
					c.addClass( classes.ascend );
				} else {
					c.removeClass( classes.ascend );
					c.addClass( classes.descend );
				}
			},
			sortBy: function( col , ascending ){
				var el = $( this ), colNum, rows;

				colNum = el[ pluginName ]( "getColumnNumber" , col );
				rows = el[ pluginName ]( "getTableRows" );
				rows = el[ pluginName ]( "sortRows" , rows , colNum , ascending, col );
				el[ pluginName ]( "replaceTableRows" , rows );
				el[ pluginName ]( "makeColDefault" , col , ascending );
			}
		};

	// Collection method.
	$.fn[ pluginName ] = function( arrg ) {
		var args = Array.prototype.slice.call( arguments , 1),
			returnVal;

		// if it's a method
		if( arrg && typeof( arrg ) === "string" ){
			returnVal = $.fn[ pluginName ].prototype[ arrg ].apply( this[0], args );
			return (typeof returnVal !== "undefined")? returnVal:$(this);
		}
		// check init
		if( !$( this ).data( pluginName + "data" ) ){
			$( this ).data( pluginName + "active", true );
			$.fn[ pluginName ].prototype._create.call( this , arrg );
		}
		return $(this);
	};
	// add methods
	$.extend( $.fn[ pluginName ].prototype, methods );

	$( document ).on( "tablesawcreate", function( e, Tablesaw ) {
		if( Tablesaw.$table.is( initSelector ) ) {
			Tablesaw.$table[ pluginName ]();
		}
	});

}( jQuery ));

;(function( win, $, undefined ){

	var MM = {
		attr: {
			init: 'data-tablesaw-minimap'
		}
	};

	function createMiniMap( $table ){

		var $btns = $( '<div class="tablesaw-advance minimap">' ),
			$dotNav = $( '<ul class="tablesaw-advance-dots">' ).appendTo( $btns ),
			hideDot = 'tablesaw-advance-dots-hide',
			$headerCells = $table.find( 'thead th' );

		// populate dots
		$headerCells.each(function(){
			$dotNav.append( '<li><i></i></li>' );
		});

		$btns.appendTo( $table.prev().filter( '.tablesaw-bar' ) );

		function showMinimap( $table ) {
			var mq = $table.attr( MM.attr.init );
			return !mq || win.matchMedia && win.matchMedia( mq ).matches;
		}

		function showHideNav(){
			if( !showMinimap( $table ) ) {
				$btns.hide();
				return;
			}
			$btns.show();

			// show/hide dots
			var dots = $dotNav.find( "li" ).removeClass( hideDot );
			$table.find( "thead th" ).each(function(i){
				if( $( this ).css( "display" ) === "none" ){
					dots.eq( i ).addClass( hideDot );
				}
			});
		}

		// run on init and resize
		showHideNav();
		$( win ).on( "resize", showHideNav );


		$table
			.bind( "tablesawcolumns.minimap", function(){
				showHideNav();
			})
			.bind( "tablesawdestroy.minimap", function(){
				var $t = $( this );

				$t.prev().filter( '.tablesaw-bar' ).find( '.tablesaw-advance' ).remove();
				$( win ).off( "resize", showHideNav );

				$t.unbind( ".minimap" );
			});
	}



	// on tablecreate, init
	$( document ).on( "tablesawcreate", function( e, Tablesaw ){

		if( ( Tablesaw.mode === 'swipe' || Tablesaw.mode === 'columntoggle' ) && Tablesaw.$table.is( '[ ' + MM.attr.init + ']' ) ){
			createMiniMap( Tablesaw.$table );
		}

	} );

}( this, jQuery ));

;(function( win, $ ) {

	var S = {
		selectors: {
			init: 'table[data-tablesaw-mode-switch]'
		},
		attributes: {
			excludeMode: 'data-tablesaw-mode-exclude'
		},
		classes: {
			main: 'tablesaw-modeswitch',
			toolbar: 'tablesaw-toolbar'
		},
		modes: [ 'stack', 'swipe', 'columntoggle' ],
		init: function( table ) {
			var $table = $( table ),
				ignoreMode = $table.attr( S.attributes.excludeMode ),
				$toolbar = $table.prev().filter( '.tablesaw-bar' ),
				modeVal = '',
				$switcher = $( '<div>' ).addClass( S.classes.main + ' ' + S.classes.toolbar ).html(function() {
					var html = [ '<label>' + Tablesaw.i18n.columns + ':' ],
						dataMode = $table.attr( 'data-tablesaw-mode' ),
						isSelected;

					html.push( '<span class="btn btn-small">&#160;<select>' );
					for( var j=0, k = S.modes.length; j<k; j++ ) {
						if( ignoreMode && ignoreMode.toLowerCase() === S.modes[ j ] ) {
							continue;
						}

						isSelected = dataMode === S.modes[ j ];

						if( isSelected ) {
							modeVal = S.modes[ j ];
						}

						html.push( '<option' +
							( isSelected ? ' selected' : '' ) +
							' value="' + S.modes[ j ] + '">' + Tablesaw.i18n.modes[ j ] + '</option>' );
					}
					html.push( '</select></span></label>' );

					return html.join('');
				});

			var $otherToolbarItems = $toolbar.find( '.tablesaw-advance' ).eq( 0 );
			if( $otherToolbarItems.length ) {
				$switcher.insertBefore( $otherToolbarItems );
			} else {
				$switcher.appendTo( $toolbar );
			}

			$switcher.find( '.btn' ).tablesawbtn();
			$switcher.find( 'select' ).bind( 'change', S.onModeChange );
		},
		onModeChange: function() {
			var $t = $( this ),
				$switcher = $t.closest( '.' + S.classes.main ),
				$table = $t.closest( '.tablesaw-bar' ).nextUntil( $table ).eq( 0 ),
				val = $t.val();

			$switcher.remove();
			$table.data( 'table' ).destroy();

			$table.attr( 'data-tablesaw-mode', val );
			$table.table();
		}
	};

	$( win.document ).on( "tablesawcreate", function( e, Tablesaw ) {
		if( Tablesaw.$table.is( S.selectors.init ) ) {
			S.init( Tablesaw.table );
		}
	});

})( this, jQuery );
;(function( $ ) {
	$(document).trigger("enhance.tablesaw");

	// hide toggle if have no option columns.
	if ($(".tablesaw").length > 0) {
	    $(".tablesaw-bar").hide();
	    $(".tablesaw").each(function () {
	        var _that = $(this);
	        var tableHeads = _that.find('thead tr th');
	        var tableBar = _that.prev();

	        if (tableHeads.length > 2) {
	            tableBar.removeAttr("style");
	        } else {
	            _that.css("min-width", "inherit");
	        }
	    });
	}

	function DeleteTableRowWithAnimation(cellButton) {
        var row = $(cellButton).closest("tr").addClass("deleted").children("td");
        setTimeout(function () {
                $(row)
                .animate({ paddingTop: 0, paddingBottom: 0 }, 500)
                .wrapInner("<div />")
                .children()
                .slideUp(500, function() { $(this).closest('tr').remove(); });
    	    }, 1000
        );
	}

	$(".lm__internal-table tbody tr .lm__form-btn").click(function () {
	    DeleteTableRowWithAnimation($(this));
	});
})( jQuery );
$(function () {
    function is_touch_device() {
        return 'ontouchstart' in window // works on most browsers
            || navigator.maxTouchPoints; // works on IE10/11 and Surface
    };

    var HeaderMainNavigation = function () {

        var toggleButton = $(".lm_toggle-menu");
        var parentList = $(".lm__main-navigation > ul > li").has("ul");
        var childList = $(".lm__main-navigation > ul li ul li").has(".lm__child-sub-navigation");
        var mainNav = toggleButton.next();
        var settingBtn = $(".lm__settings > a");
        var subNavigation = $(".lm__sub-navigation");
        var settingWrapper = $(".lm__user-avatar-wrapper");

        var removeActiveState = function () {
            mainNav.removeAttr("style");
            parentList.find('ul').slideUp("slow", function () {
                $(this).prev().removeClass('minus-sign').addClass('plus-sign');
                $(this).removeAttr("style");
            });
            //childList.find('ul').slideUp("slow", function () {
            //    $(this).prev().find('.toggle-icon').removeClass('minus-sign').addClass('plus-sign');
            //    $(this).removeAttr("style");
            //});
            toggleButton.removeClass('active');
            $(".humb").removeClass('open');
            settingBtn.parent().removeClass('active').find(".lm_sub-settings").slideUp();
            $(".lm__navigation-wrapper.showed-menu").removeClass("showed-menu");
            settingWrapper.find(".lm__user-avatar").next().slideUp();
            $(".user-info-wrapper > a").next().slideUp();
        }

        var addSettingEvent = function () {
            var avatar = settingWrapper.find(".lm__user-avatar");
            avatar.click(function () {
                removeActiveState();
                var sub = $(this).next();

                $(this).toggleClass("active");
                if (sub.css('display') == 'none') {
                    sub.slideDown();
                } else {
                    sub.slideUp();
                }
                return false;
            });
        };

        var addUserInfoEvent = function () {
            var userInfoWrapper = $(".user-info-wrapper > a");
            userInfoWrapper.click(function () {
                removeActiveState();
                if ($(this).next().css("display") == "none") $(this).next().slideDown();
                else $(this).next().slideUp();
                return false;
            });
        }

        var addToggleMenu = function () {

            // Remove padding from settings dot button if anchor link has no content.
            if ($(".lm__settings > a").text() == "") {
                $(".lm__settings > a").css({ padding: "0 25px" });
            }

            $(document).on("click", function () {
                removeActiveState();
            }).on('touchstart', function (e) {
                var $this = $(e.target);
                if ($this.attr('class') == 'lm__user-info' || $($this).parents('.user-info-wrapper').length > 0) {
                    return;
                } else {
                    $(".user-info-wrapper > a").next().slideUp();
                }
            });

            // Toggle Settings
            settingBtn.off('click').on('click', function (e) {
                e.stopPropagation();
                removeActiveState();
                var subMenu = $(this).next();

                parentList.find('.lm_sub-settings').slideUp();
                if (subMenu.css('display') == 'none') {
                    subMenu.slideDown('fast', function () {
                        if ($(this).is(':visible'))
                            $(this).css('display', 'inline-block');
                    });
                    $(this).parent().addClass('active');
                } else {
                    subMenu.slideUp('fast');
                    $(this).parent().removeClass('active');
                }

                toggleButton.removeClass('active');
                $(".humb").removeClass('open');
                subNavigation.slideUp("slow", function () {
                    $(this).removeAttr("style");
                });

                return false;
            });

            // Toggle menu navigation
            toggleButton.off('click').on("click", function (e) {
                e.stopPropagation();

                var $this = $(this);
                var subMenu = $this.next();
                if (is_touch_device()) {
                    subMenu.css({
                        'max-height': 'calc(100vh - 100px)',
                        'overflow': 'hidden',
                        'overflow-y': 'visible'
                    });
                }
                if (subMenu.css('display') == 'none') {
                    subMenu.slideDown('fast');
                    $this.addClass('active').find('.humb').addClass('open');
                } else {
                    subMenu.slideUp('fast');
                    $this.removeClass('active').find('.humb').removeClass('open');
                }

                settingBtn.parent().removeClass('active').find('>a').next().slideUp();
                return false;
            });

            // Toggle sub-menu
            parentList.each(function () {
                $(this).find("> a").off('click').on("click", function (e) {
                    var $this = $(this);
                    parentList.not($this.parent()).find('ul').slideUp(function () {
                        $(this).prev().removeClass('minus-sign').addClass('plus-sign');
                    });
                    settingBtn.parent().removeClass('active').find(".lm_sub-settings").slideUp();
                    $this.next().slideToggle(function () {
                        if ($(this).css('display') == 'none') {
                            $this.removeClass('minus-sign').addClass('plus-sign');
                        } else {
                            $this.removeClass('plus-sign').addClass('minus-sign');
                        }
                    });
                    if ($(window).width() < 1083) {
                        $this.next().find(".lm__child-sub-navigation").slideToggle();
                    }
                    return false;
                });
            });

            //childList.each(function () {
            //    $(this).find(">a .toggle-icon").off('click').on("click", function (e) {
            //        var $this = $(this);
            //        childList.not($this.parent().parent()).find('ul').slideUp(function () {
            //            $(this).prev().find('.toggle-icon').removeClass('minus-sign').addClass('plus-sign');
            //        });
            //        $this.parent().next().slideToggle(function () {
            //            if ($(this).css('display') == 'none') {
            //                $this.removeClass('minus-sign').addClass('plus-sign');
            //            } else {
            //                $this.removeClass('plus-sign').addClass('minus-sign');
            //            }
            //        });
            //        return false;
            //    });
            //});
        }

        var resizeSubNav = function () {
            var wHeight = $(window).height() - 50;
            var subHeight = $(".lm_sub-settings").height();
            if (subHeight > wHeight) $(".lm_sub-settings").css({ height: $(window).height() - 50 + "px" });
            else $(".lm_sub-settings").css('height', 'auto');
            //$(".humb").removeClass('open');
        }

        var setIconForParentMenu = function () {
            // set icon for menu which have sub menu
            parentList.each(function () {
                var $this = $(this);
                if ($this.has('.lm__sub-navigation')) {
                    $this.find('>a').addClass('plus-sign');
                }
            });

            //childList.each(function () {
            //    var $this = $(this);
            //    if ($this.has('.lm__child-sub-navigation')) {
            //        $this.find('>a .toggle-icon').addClass('plus-sign');
            //    }
            //});
        }

        var responsiveIcon = function () {
            if ($(window).width() >= 1083) {
                parentList.find('>a').addClass('hide-icon');
                //childList.find('>ul').hide();
            }
            else {
                parentList.find('>a').removeClass('hide-icon');
                //childList.find('>ul').removeAttr('style');
            }
        }

        return {
            init: function () {
                setIconForParentMenu();
                responsiveIcon();

                removeActiveState();
                addToggleMenu();
                addSettingEvent();
                addUserInfoEvent();
                resizeSubNav();

                $(window).on('resize orientationchange', function () {
                    if ($(window).width() >= 1083) $(".lm__main-navigation > ul").removeAttr("style");

                    resizeSubNav();
                    responsiveIcon();
                });
            }
        }
    };

    var SidebarNavigation = function () {

        var sidebarNav = $(".lm__category-nav > ul > li").has("ul");

        var addAccordion = function () {
            sidebarNav.each(function () {
                if ($(this).find("ul li.active").length > 0 || $(this).hasClass('active')) $(this).addClass('open').find('ul').slideDown();
                else $(this).removeClass('open');
            });
        }

        return {
            init: function () {
                addAccordion();
            }
        }
    }

    var MessageHub = function () {
        var getTotalUnreadMessage = function () {
            $.ajax({
                dataType: "json",
                url: '/api/user/get-inbox-statistics',
                type: 'post',
                cache: false,
                success: function (data) {
                    if (data && data.totalUnRead > 0) {
                        if (!$('#totalUnread').hasClass('lm__messages-count')) {
                            $('#totalUnread').addClass('lm__messages-count');
                        }
                        $('#totalUnread').html(data.totalUnRead);
                    }
                    else {
                        if ($('#totalUnread').hasClass('lm__messages-count')) {
                            $('#totalUnread').removeClass('lm__messages-count');
                        }
                        $('#totalUnread').html('');
                    }

                    if (data && data.totalStarred > 0) {
                        if (!$('#totalStarred').hasClass('lm__messages-stat-count')) {
                            $('#totalStarred').addClass('lm__messages-stat-count');
                        }
                        $('#totalStarred').html(data.totalStarred);
                    }
                    else {
                        if ($('#totalStarred').hasClass('lm__messages-stat-count')) {
                            $('#totalStarred').removeClass('lm__messages-stat-count');
                        }
                        $('#totalStarred').html('');
                    }

                }
            });
        }

        return {
            init: function () {
                getTotalUnreadMessage();
            }
        }
    }


    var CustomerList = function () {
        var registerEventHandler = function () {
            var customerContainer = $('.user-id__wrapper.lm__radio');
            if (!!customerContainer) {
                customerContainer.on("click", function () {
                    var newCustomerNo = $(this).attr("data-customer-no");
                    var currentActiveNo = $('a.lm__user-info').attr("data-active-No");
                    if (newCustomerNo === currentActiveNo) {
                        return;
                    }
                    var newCustomerName = $(this).attr("data-customer-name");

                    // udpate newvalue for view;
                    $('.lm__user-info .lm__user-id').html(newCustomerNo);
                    $('a.lm__user-info').attr("data-active-No", newCustomerNo);
                    $('.lm__user-info .lm__user-summary').html(newCustomerName);

                    $("#newCustomerNumber").val(newCustomerNo);
                    //$("#referenceLink").val(window.location.href);
                    $("#customerNumberForm").submit();

                });
            }
        }

        return {
            init: function () {
                registerEventHandler();
            }
        }
    }

    // disable :hover on touch devices
    // based on https://gist.github.com/4404503
    // via https://twitter.com/javan/status/284873379062890496
    // + https://twitter.com/pennig/status/285790598642946048
    // re http://retrogamecrunch.com/tmp/hover
    // NOTE: we should use .no-touch class on CSS
    // instead of relying on this JS code
    function removeHoverCSSRule() {
        try {
            var ignore = /:hover/;
            for (var i = 0; i < document.styleSheets.length; i++) {
                var sheet = document.styleSheets[i];
                if (!sheet.cssRules) {
                    continue;
                }
                for (var j = sheet.cssRules.length - 1; j >= 0; j--) {
                    var rule = sheet.cssRules[j];
                    if (rule.type === CSSRule.STYLE_RULE && ignore.test(rule.selectorText)) {
                        sheet.deleteRule(j);
                    }
                }
            }
        }
        catch(e) {
        }
    }

    function adjustStickyHeaderWidth() {
        var contentWidth = $(".lm__contents").width();
        var stickyHeader = $(".sticky-header");
        var leftColumn = $(".internal-page > .wrapper >.layout > .layout__item:first-child").width();
        var internalNav = $(".internal-navigation");

        if (stickyHeader.hasClass("fixed")) {
            stickyHeader.css({ width: contentWidth + "px" });
        } else {
            stickyHeader.removeAttr("style");
        }
        internalNav.css({ "width": leftColumn });
    }

    $(document).ready(function () {
        if ($(".lm__navigation-wrapper") != undefined && $(".lm__navigation-wrapper") != null && $(".lm__navigation-wrapper").length > 0) {
            // Client Sites - Main Navigation
            if ($(".lm__navigation-wrapper").length > 0) {
                $(window).scroll(function () {
                    if ($(this).scrollTop() > $(".lm__navigation-wrapper").offset().top) {
                        $(".lm__navigation").addClass("fixed");
                    }
                    if ($(this).scrollTop() <= $(".lm__navigation-wrapper").offset().top) {
                        $(".lm__navigation").removeClass("fixed");
                    }
                });
            }

            var headerMainNav = new HeaderMainNavigation();
            headerMainNav.init();

            var sidebarNav = new SidebarNavigation();
            sidebarNav.init();
            var messageHub = new MessageHub();
            messageHub.init();

            // change customer number
            if (!!$("#customer_list_selection")) {
                $("#customer_list_selection").on("change", function (e) {
                    var form = $(e.target).parents("form");
                    form.submit();
                });
            }

            var customerList = new CustomerList();
            customerList.init();

            if (is_touch_device()) {
                removeHoverCSSRule();
            }
        }

        // Internal Sites - Search box
        if ($(".lm__header.internal-page").length > 0) {


            $(window).scroll(function () {
                if ($(this).scrollTop() > $(".lm__header.internal-page").offset().top) {
                    $(".sticky-header").addClass("fixed")
                        .css({ width: $(".lm__contents").width() + "px" });
                }
                if ($(this).scrollTop() <= $(".lm__header.internal-page").offset().top) {
                    $(".sticky-header").removeClass("fixed")
                        .removeAttr("style");
                }
            });

            $(window).resize(function() {
                adjustStickyHeaderWidth();
            })
            adjustStickyHeaderWidth();
        }
        if (GroCommon) {
            GroCommon.handleExternalLink();
        }
    });

});


$(document).ready(function () {
    $("#header-internal-search .dropdown li a").click(function () {
        var searchOption = $(this).attr("data-value");
        $("#searchOption").val(searchOption);
    });
    $("button#internal-search").click(function () {
        var searchKey = $("#input-key-search").val();
        if (searchKey.trim() !== "") {
            $("#searchKey").val(searchKey);
            $("#internalSearchForm").submit();
        }
    });

    $("header.internal-page a.close-btn").click(function() {
        if ($(this).closest("#customer-popup").hasClass('open')) {
            $.ajax({
                dataType: "json",
                url: '/internal-portal/close-customer-session',
                type: 'get',
                cache: false,
                success: function (data) {
                    if (data) {
                        // reload the page
                        location.reload();
                    }
                }
            });
        }
    });

});




/* PROGRESS CIRCLE COMPONENT */
(function ($) {

    $.fn.circliful = function (options, callback) {

        var settings = $.extend({
            // These are the defaults.
            startdegree: 0,
            fgcolor: "#556b2f",
            bgcolor: "#eee",
            fill: false,
            width: 15,
            dimension: 200,
            fontsize: 15,
            percent: 50,
            animationstep: 1.0,
            iconsize: '20px',
            iconcolor: '#999',
            border: 'default',
            complete: null,
            bordersize: 10
        }, options);

        return this.each(function () {

            var customSettings = ["fgcolor", "bgcolor", "fill", "width", "dimension", "fontsize", "animationstep", "endPercent", "icon", "iconcolor", "iconsize", "border", "startdegree", "bordersize"];

            var customSettingsObj = {};
            var icon = '';
            var endPercent = 0;
            var obj = $(this);
            var fill = false;
            var text, info;

            obj.addClass('circliful');

            checkDataAttributes(obj);

            if (obj.data('text') != undefined) {
                text = obj.data('text');

                if (obj.data('icon') != undefined) {
                    icon = $('<i></i>')
                        .addClass('fa ' + $(this).data('icon'))
                        .css({
                            'color': customSettingsObj.iconcolor,
                            'font-size': customSettingsObj.iconsize
                        });
                }

                if (obj.data('type') != undefined) {
                    type = $(this).data('type');

                    if (type == 'half') {
                        addCircleText(obj, 'circle-text-half', (customSettingsObj.dimension / 1.45));
                    } else {
                        addCircleText(obj, 'circle-text', customSettingsObj.dimension);
                    }
                } else {
                    addCircleText(obj, 'circle-text', customSettingsObj.dimension);
                }
            }

            if ($(this).data("total") != undefined && $(this).data("part") != undefined) {
                var total = $(this).data("total") / 100;

                percent = (($(this).data("part") / total) / 100).toFixed(3);
                endPercent = ($(this).data("part") / total).toFixed(3)
            } else {
                if ($(this).data("percent") != undefined) {
                    percent = $(this).data("percent") / 100;
                    endPercent = $(this).data("percent")
                } else {
                    percent = settings.percent / 100
                }
            }

            if ($(this).data('info') != undefined) {
                info = $(this).data('info');

                if ($(this).data('type') != undefined) {
                    type = $(this).data('type');

                    if (type == 'half') {
                        addInfoText(obj, 0.9);
                    } else {
                        addInfoText(obj, 1.25);
                    }
                } else {
                    addInfoText(obj, 1.25);
                }
            }

            $(this).width(customSettingsObj.dimension + 'px');

            var canvas = $('<canvas></canvas>').attr({
                width: customSettingsObj.dimension,
                height: customSettingsObj.dimension
            }).appendTo($(this)).get(0);

            var context = canvas.getContext('2d');
            var container = $(canvas).parent();
            var x = canvas.width / 2;
            var y = canvas.height / 2;
            var degrees = customSettingsObj.percent * 360.0;
            var radians = degrees * (Math.PI / 180);
            var radius = canvas.width / 2.5;
            var startAngle = 2.3 * Math.PI;
            var endAngle = 0;
            var counterClockwise = false;
            var curPerc = customSettingsObj.animationstep === 0.0 ? endPercent : 0.0;
            var curStep = Math.max(customSettingsObj.animationstep, 0.0);
            var circ = Math.PI * 2;
            var quart = Math.PI / 2;
            var type = '';
            var fireCallback = true;
            var additionalAngelPI = (customSettingsObj.startdegree / 180) * Math.PI;

            if ($(this).data('type') != undefined) {
                type = $(this).data('type');

                if (type == 'half') {
                    startAngle = 2.0 * Math.PI;
                    endAngle = 3.13;
                    circ = Math.PI;
                    quart = Math.PI / 0.996;
                }
            }

            /**
             * adds text to circle
             *
             * @param obj
             * @param cssClass
             * @param lineHeight
             */
            function addCircleText(obj, cssClass, lineHeight) {
                $("<span></span>")
                    .appendTo(obj)
                    .addClass(cssClass)
                    .text(text)
                    .prepend(icon)
                    .css({
                        'line-height': lineHeight + 'px',
                        'font-size': customSettingsObj.fontsize + 'px',
                        'color': parseInt(text) > 0 ? customSettingsObj.fgcolor : customSettingsObj.bgcolor
                    });
            }

            /**
             * adds info text to circle
             *
             * @param obj
             * @param factor
             */
            function addInfoText(obj, factor) {
                $('<span></span>')
                    .appendTo(obj)
                    .addClass('circle-info-half')
                    .css(
                        'line-height', (customSettingsObj.dimension * factor) + 'px'
                    )
                    .text(info);
            }

            /**
             * checks which data attributes are defined
             * @param obj
             */
            function checkDataAttributes(obj) {
                $.each(customSettings, function (index, attribute) {
                    if (obj.data(attribute) != undefined) {
                        customSettingsObj[attribute] = obj.data(attribute);
                    } else {
                        customSettingsObj[attribute] = $(settings).attr(attribute);
                    }

                    if (attribute == 'fill' && obj.data('fill') != undefined) {
                        fill = true;
                    }
                });
            }

            /**
             * animate foreground circle
             * @param current
             */
            function animate(current) {

                context.clearRect(0, 0, canvas.width, canvas.height);

                context.beginPath();
                context.arc(x, y, radius, endAngle, startAngle, false);

                context.lineWidth = customSettingsObj.bordersize + 1;

                context.strokeStyle = customSettingsObj.bgcolor;
                context.stroke();

                if (fill) {
                    context.fillStyle = customSettingsObj.fill;
                    context.fill();
                }

                context.beginPath();
                context.arc(x, y, radius, -(quart) + additionalAngelPI, ((circ) * current) - quart + additionalAngelPI, false);

                if (customSettingsObj.border == 'outline') {
                    context.lineWidth = customSettingsObj.width + 13;
                } else if (customSettingsObj.border == 'inline') {
                    context.lineWidth = customSettingsObj.width - 13;
                }

                context.strokeStyle = customSettingsObj.fgcolor;
                context.stroke();

                if (curPerc < endPercent) {
                    curPerc += curStep;
                    requestAnimationFrame(function () {
                        animate(Math.min(curPerc, endPercent) / 100);
                    }, obj);
                }

                if (curPerc == endPercent && fireCallback && typeof (options) != "undefined") {
                    if ($.isFunction(options.complete)) {
                        options.complete();

                        fireCallback = false;
                    }
                }
            }

            animate(curPerc / 100);

        });
    };

    $(document).ready(function () {
        $('.circle-progress').circliful();
    });
}(jQuery));


$(function () {
    var modal = $(".lm__modal-alert");
    var containerData = modal.find(".lm__modal-dialog");
    var agreementData;

    var AgreementData = function (url) {
        var modal = $(".lm__modal-alert");
        var closeModal = modal.find(".modal-close");
        var loadingIcon = modal.find(".loader-wrapper");
        var confirm = modal.find(".success-confirm");

        var CloseModalAlert = function () {
            modal.fadeOut(function () {
                containerData.find(".modal-header-title, .modal-content").remove();
            });

            loadingIcon.fadeIn();

            return false;
        }

        var addEvents = function () {
            modal.bind("click", CloseModalAlert);
            containerData.bind("click", function (event) { event.preventDefault(); return false; });
            closeModal.bind("click", CloseModalAlert);
            confirm.bind("click", CloseModalAlert);
        }

        return {
            init: function () {
                addEvents();
                return true;
            }
        }
    };

    function bindData(data) {
        var loadingIcon = modal.find(".loader-wrapper");
        $(data).insertBefore(".button-confirm");

        loadingIcon.fadeOut();
    }

    $(document).ready(function () {
        
        // enhance table saw to make responsive table
        $(document).trigger("enhance.tablesaw");

        agreementData = new AgreementData();
        agreementData.init();

        if ($(".showAnalyze")) $(".showAnalyze").bind('click',
            function () {
                var url = '/api/agreement/farmsample/' + $(this).attr("data-id").toString();
                $.ajax({
                    dataType: "html",
                    url: url,
                    success: function (data) {
                        bindData(data);
                    }
                });

                modal.fadeIn();

                return false;
            });

        if ($(".showPriceHedging")) $(".showPriceHedging").bind('click',
            function () {
                var url = '/api/agreement/price-hedging/' + $(this).attr("data-id").toString();
                $.ajax({
                    dataType: "html",
                    url: url,
                    success: function (data) {
                        bindData(data);
                    }
                });

                modal.fadeIn();

                return false;
            });

        if ($(".showWeighInMoreInfo")) $(".showWeighInMoreInfo").bind('click',
            function () {

                var url = '/api/weigh-in/more-info/' + $(this).attr("data-id").toString();
                url += "?date=" + encodeURIComponent($(this).attr("data-date")) + "&sort=" + encodeURIComponent($(this).attr("data-sort"));
                $.ajax({
                    dataType: "html",
                    url: url,
                    success: function (data) {
                        bindData(data);
                    },
                    error: function () {
                        modal.fadeOut();
                    }
                });

                modal.fadeIn();

                return false;
            });

        if ($(".showWeighInAnalyze")) $(".showWeighInAnalyze").bind('click',
            function () {
                var url = '/api/weigh-in/analyze/' + $(this).attr("data-id").toString();
                url += "?date=" + encodeURIComponent($(this).attr("data-date")) + "&sort=" + encodeURIComponent($(this).attr("data-sort"));
                $.ajax({
                    dataType: "html",
                    url: url,
                    success: function (data) {
                        bindData(data);
                    },
                    error: function()
                    {
                        modal.fadeOut();
                    }
                });
                modal.fadeIn();
                return false;
            });
    });
});

(function () {
    $(document).ready(function () {
        var chartDiv = document.getElementById("lm__chart-dygraphs");
        var labelDiv = document.getElementById('lm__chart-status');
        if (window.Dygraph && chartDiv && labelDiv) {
            g2 = new Dygraph(
                document.getElementById("lm__chart-dygraphs"),
                chartDiv.dataset.rowdata,
                {
                    labels: [
                        "Date", "Höstvete Kvarn", "Höstvete Foder", "Maltkorn", "Oljeväxter", "Grynhavre"
                    ],
                    labelsDiv: document.getElementById('lm__chart-status'),
                    //labelsSeparateLines: false,
                    labelsKMB: false,
                    legend: 'always',
                    colors: ["#0e8013", "#808080", "#030389", "#fc0019", "#aed8e5"],
                    width: "100%",
                    height: 540,
                    title: chartDiv.dataset.title,
                    //xlabel: 'Date',
                    //ylabel: 'Count',
                    axisLineColor: 'white',
                    //showLabelsOnHighlight: false
                    // drawXGrid: false
                }
            );
        }

        var selectMonthElement = document.getElementById("selectMonthPeriod");
        if (selectMonthElement) {
            $("#selectMonthPeriod > li > ul > li > a").click(function () {
                var dataValueType = $(this).parent().attr("data-value");
                location.href = window.location.origin + location.pathname + "?period=" + dataValueType;
            });
        }
    });
})();

function setDropdown(obj, type) {
    obj.find(".showcase > a").unbind('click').click(function () {
        var sub = $(this).next();
        $(".dropdown").not(sub).hide();
        if (sub.css('display') == 'none') {
            sub.show();
        } else {
            sub.hide();
        }
        return false;
    });

    obj.find(".showcase .dropdown li a").click(function () {
        var data = $(this).parent().attr("data-value");
        var dataText = $(this).text();
        $(".dropdown li").removeClass("selected");
        $(this).parent().addClass("selected");
        $(this).parents(".showcase").find(">a").html((type === "type-2") ? data : dataText);
        $(this).parents(".showcase").find(">a").html(dataText);
        $(this).parents(".showcase").attr("data-value", data);
        //$(this).parents(".showcase").find("input.form-element").val(data);
        $(this).parents(".showcase").find(">input[type=hidden]").val(data);
        $(this).parents(".dropdown").hide();

        if ($(this).parent().data('value') === 'Annan') {
            $(this).parents('.model-block').find('input[type="text"]').show();
            $(this).parents('ul.lm__form-dropdown.model-dropdown').hide();
        }
        return false;
    });
}

$(function () {
    function addDataPickerHandler() {
        if ($.fn.datepicker) {
            // Set default options for datepicker
            $.datepicker.setDefaults({
                monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec"],
                monthNames: ['Januari', 'Februari', 'Mars', 'April', 'Maj', 'Juni', 'Juli', 'Augusti', 'September', 'Oktober', 'November', 'December'],
                dayNamesMin: ["Sö", "Må", "Ti", "On", "To", "Fr", "Lö"],
                firstDay: 0,
                showOtherMonths: true,
                selectOtherMonths: true,
                dateFormat: "yy-mm-dd"
            });

            $(".datepicker").datepicker({
                defaultDate: new Date(),
                onSelect: function (newText) {
                    // compare the new value to the previous one
                    if ($(this).data('previous') != newText) {
                        // do whatever has to be done, e.g. log it to console
                        //console.log('changed to: ' + newText);
                    }
                },
            monthNames: ["Januari", "Februari", "Mars", "April", "Maj", "Juni", "Juli", "Augusti", "September", "Oktober", "November", "December"],
            dayNamesMin: ["Sö", "Må", "Ti", "On", "To", "Fr", "Lö"],
            firstDay: 1
            }).datepicker("setDate", new Date())
            .on('change', function () {
                var $this = $(this);
                var validDate = !/Invalid|NaN/.test(new Date($this.val()).toString());
                var validDateRegex = /^\d\d\d\d-(0?[1-9]|1[0-2])-(0?[1-9]|[12][0-9]|3[01])$/ig.test($this.val());

                if (!validDateRegex || !validDate) {
                    //$this.addClass("error").next().show();
                    $this.next().show();
                } else {
                    //$this.removeClass("error").next().hide();
                    $this.next().hide();
                }
            });
        }
    }

    function addDropDownHandler() {
        var dropdown = $(".lm__form-dropdown");

        $(document).on('click touchstart', function (e) {
            var $this = $(e.target);
            if ($this.attr('class') == 'dropdown' || $($this).parents('.lm__form-dropdown').length > 0) {
                return;
            } else {
                $(".dropdown").hide();
            }
        });

        dropdown.each(function () {
            $(this).find(".showcase > a").click(function () {
                var sub = $(this).next();
                $(".dropdown").not(sub).hide();
                if (sub.css('display') == 'none') {
                    sub.show();
                } else {
                    sub.hide();
                }
                return false;
            });

            setDropdown($(this), 'type-2');
            setDropdown($(this), 'type-3');
        });
    }

    addDataPickerHandler();
    addDropDownHandler();
});

$(function () {
    $(document).ready(function () {
        if ($("#calculate_delivery_fee")) {
            if (jQuery.validator) {
                $("#calculateForm").validate({
                    ignore: [],
                    errorElementClass: 'error',
                    errorClass: 'error-item',
                    errorElement: 'span',
                    rules: {
                        LorryType: "required",
                        DeliveryAdress: {
                            required: true
                        },
                        Quantity: {
                            required: true,
                            number: true,
                            min: 1
                        },
                        DeliveryDate: {
                            required: true,
                            swedishDate: true
                        },
                        Artikel_Frakt: {
                            required: true
                        }
                    },
                    messages: {
                        LorryType: {
                            required: "Du måste välja leveranssätt"
                        },
                        DeliveryAdress: {
                            required: "Du måste välja en leveransadress"
                        },
                        Quantity: {
                            required: "Du måste ange en kvantitet större än 0",
                            number: "Ange ett numeriskt värde",
                            min: "Du måste ange en kvantitet större än 1"
                        },
                        DeliveryDate: {
                            required: "Du måste ange ett giltigt datum",
                            swedishDate: "Du måste ange ett giltigt datum"
                        },
                        Artikel_Frakt: {
                            required: "Du måste välja en artikel"
                        }
                    },
                    highlight: function (element, errorClass, validClass) {
                        $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
                    },
                    unhighlight: function (element, errorClass, validClass) {
                        $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
                    },
                });
            }

            $("#calculate_delivery_fee").bind("click", function (e) {
                if (!$("#calculateForm").valid()) {
                    return;
                };
                if ($('.calculate_result .calculate_fee')) {
                    $('.calculate_result .calculate_fee').html('...');
                }
                $("#loaderCalculation").show();
                var url = "/api/deliveryfee/caluculate";
                var calculateObj = getCalculateObject();

                $.ajax({
                    type: "post",
                    data: calculateObj,
                    dataType: "html",
                    url: url,
                    success: function (data) {
                        //console.log(url);
                        $(".calculate_result").html(data);
                    },
                    complete: function (jqXHR) {
                        $('#loaderCalculation').hide();
                    },
                    error: function () {
                        $(".calculate_result").html('');
                    }
                });

            });
        }

        function getCalculateObject() {
            return {
                supplier: $('.calculate_supplier').val(),
                lorryType: $('#LorryType').val(),
                deliveryAddressId: $('#DeliveryAdress').val(),
                quantity: $('.calculate_quantity').val(),
                deliveryDate: $('.calculate_deliveryDate').val(),
                itemId: $('#Artikel_Frakt').val()
            };
        }
    });

});
jQuery(function() {

    var DeliveryNote = DeliveryNote || (function() {

        var $orderLink = jQuery('.deliveryNote a.showPdf');

        var getPdf = function(orderNumber, fabricId, rowNumber) {
            GroCommon.blockUI();
            var url = '/api/delivery-note/get-pdf' + '?orderNumber=' + orderNumber + '&fabricId=' + fabricId + '&rowNumber=' + rowNumber;

            jQuery.ajax({
                dataType: "json",
                url: url,
                success: function(data) {
                    GroCommon.unblockUI();

                    if (data != null && data.pdfData != null) {

                        var pdfData = data.pdfData;
                        var $dialog = $(".pdf-getting-error");
                        if (pdfData && pdfData.trim().endsWith(".pdf")) {
                            window.open('http://wcf.lantmannenlantbruk.se/pdf/' + pdfData);
                        } else {
                            var errorMessage = "Ett fel uppstod när pdf skulle laddas";
                            $('.success-header-title', $dialog).html(errorMessage);
                            $dialog.fadeIn();
                        }

                        //if (pdfData.length == 1) {
                        //    if (pdfData[0].indexOf('.pdf') > -1) {
                        //        window.open('http://wcf.lantmannenlantbruk.se/pdf/' + pdfData[0]);
                        //    } else {
                        //        // error
                        //        alert(pdfData[0]);
                        //    }
                        //} else {
                        //    //Todo
                        //}
                    }
                },
                error: function(e) {
                    GroCommon.unblockUI();
                    //console.log("Error occured:" + ':\nReadystate:' + XMLHttpRequest.readyState + '\nResponsetext:' + XMLHttpRequest.responseText + '\nResponseXML:' + XMLHttpRequest.responseXML + '\nStatus:' + XMLHttpRequest.status + '\nStatusText:' + XMLHttpRequest.statusText);
                }
            });
        }

        var init = function() {
            $orderLink.click(function() {

                var orderNumber = jQuery(this).attr('cdata-orderNumber');
                var activePdf = jQuery(this).attr('cdata-activePdf');
                var fabricId = jQuery(this).attr('cdata-fabricID');
                var rowNumber = jQuery(this).attr('cdata-rowNumber');
                var customerNumber = jQuery(this).attr('cdata-customerNumber');
                var system = jQuery(this).attr('cdata-system');

                if (activePdf == 'True') {
                    getPdf(orderNumber, fabricId, rowNumber);
                } else {
                    var url = null;
                    switch (system) {
                    case '3':
                    {
                        break;
                    }
                    case '4':
                    {
                        break;
                    }
                    default:
                    {
                        break;
                    }
                    }
                }
            });
        };
        return {
            init: init
        };
    })();

    $(document).ready(function() {
        DeliveryNote.init();
    });
});




$(function () {

    var SokGrobarhet = function () {

        var container = $(".lm__sok-grobarhet");
        var infoBtn = container.find(".lm__info");
        var modal = container.find(".lm__modal-alert");
        var messageClosable = $(".lm__message-closable .close-btn, .lm__modal-alert .button-confirm .success-confirm-inform");
        var informModal = $(".lm__information-modal__wrapper");

        var modalPopup = function () {
            infoBtn.click(function () {
                modal.fadeIn();
            });
        };

        var closeMessage = function () {
            messageClosable.click(function () {
                $(this).parents(".lm__modal-alert, .lm__message-closable").fadeOut();
            });
        };

        var informModalEvent = function () {
            $(".lm__information-modal__close-btn").click(function () {
                $(this).parents(".lm__information-modal__wrapper").addClass("hidden");
                $(this).parents(".lm__modal-alert").fadeOut();
                return false;
            });
        };

        return {
            init: function () {
                modalPopup();
                closeMessage();
                informModalEvent();
                return false;
            }
        }
    }

    var CommonModal = function () {
        var modal = $(".lm__modal__wrapper");
        var closeXBtn = modal.find(".lm__wide-modal__close-btn");
        var closeBtn = modal.find(".lm__btn-close");

        var closeModalEvent = function () {
            closeXBtn.click(function() {
                $(this).parents(".lm__modal__wrapper").addClass("hidden");
                //console.log("lm-btn");
                return false;
            });

            closeBtn.click(function() {
                $(this).parents(".lm__modal__wrapper").addClass("hidden");
                return false;
            });
        }

        return {
            init: function() {
                closeModalEvent();
                return false;
            }
        }
    }

    $(document).ready(function () {
        var sok = new SokGrobarhet();
        sok.init();

        var modal = new CommonModal();
        modal.init();
    });
});

$(function () {

    $(document).ready(function () {
        $("form").on("change", ".file-upload-field", function(){
            $(this).parent(".file-upload-wrapper").attr("data-text", $(this).val().replace(/.*(\/|\\)/, '') );
        });

        if ($('#attachments').length > 0) $('#attachments').filer({
		    showThumbs: true,
		    addMore: true,
		    allowDuplicates: false
        });
    });

});
$(function () {

    var MittKonto = function () {
        var uploadBtn = $(".lm__user-inform__wrapper .lm__avatar__upload-btn");
        var removeAvatarBtn = $(".lm__user-inform__wrapper .popup-sub li .remove-avatar");
        var uploadBtns = $(".file-upload-input");

        var addUploadPopupEvent = function () {
             $(document).on("click", function () {
                 $('.popup-sub').fadeOut();
             });
             $(".popup-sub").click(function () { return false; });
            uploadBtn.click(function () {
                var popupSub = $(this).parent();
                if (popupSub.has('.popup-sub')) {
                    popupSub.find('.popup-sub').fadeToggle();
                }
                return false;
            });

            //removeAvatarBtn.click(function () {
            //    var avatar = $(this).parents(".lm__avatar").find(".avatar-img");
            //    avatar.attr("src", $(this).attr("default-avatar"));
            //    return false;
            //});
        };

        var addUploadImageEvent = function () {
            uploadBtns.each(function (event) {
                $(this).change(function (event) {
                    var imgEle = $(this).parent().find(".avatar-img");
                    imgEle.attr('src', URL.createObjectURL(event.target.files[0]));
                });
            });
        }

        return {
            init: function () {
                addUploadPopupEvent();
                //addUploadImageEvent();
                return false;
            }
        }
    };

    $(document).ready(function () {
        //var mit = new MittKonto();
        //mit.init();
    });

});
$(function () {
    var MachineListing = function () {

        var toggleViewBtns = $("#toggle-view a");
        var toggleView = $(".machine-list");
        var machineItems = $('.machine-item');

        var ToggleViewEvent = function () {
            toggleViewBtns.each(function () {
                var $this = $(this);
                if ($this.hasClass('active')) {
                    $this.css({backgroundColor: '#fff'});
                }
            });
            toggleViewBtns.on('click', function (e) {
                e.preventDefault();

                var $this = $(this);
                var tgr = $this.data('target');

                // Switch view
                toggleView.addClass((tgr == 'list') ? 'list-view' : '');
                toggleView.removeClass((tgr == 'grid') ? 'list-view' : '');

                // Change active class for selected btn
                toggleViewBtns.removeClass('active').removeAttr('style');
                $this.addClass('active').css({ backgroundColor: '#fff' });

                return false;
            });
        };

        return {
            init: function () {
                ToggleViewEvent();

                //$(window).on('resize orientationchange', function () {
                //    if (window.innerWidth < 768) {
                //        toggleView.removeClass('list-view');
                //        toggleViewBtns.removeClass('active').first().addClass('active');
                //    }
                //});
            }
        }
    };

    $(document).ready(function () {
        /** ======================= DEPREACATED ======================= */
        $('.lm__list-type a').on('click', function () {
            var dataLayout = $(this).attr("data-layout");

            // Change active layout button
            $('.lm__list-type a').removeClass('active');
            $(this).addClass('active');

            // Change layout
            $(".list-maskin").removeClass("list-type").removeClass("grid-type").addClass(dataLayout).show();

            //$(".list-maskin").hide();
            //$("."+dataLayout).show();
        });

        $('.lm__list-type a.active').click();

        $('#checkRegisterBtn').click(function () {
            $('.lm__modal-alert').show();
        });

        /** ======================= DEPREACATED ======================= */

        var machineListing = new MachineListing()
            machineListing.init();
    });

});

if (jQuery.validator) {
    jQuery.validator.addMethod("subItemRequired", function (value, element, method) {
        return method;
    }, '');
    jQuery.validator.addMethod("regex", function (value, element, regexpr) {
        return this.optional(element) || regexpr.test(value);
    }, 'Please enter a valid value.');
}

jQuery(function () {
    MyAccountPage.init();
    HandinSignInInformationPage.init();
    CompanyInformation.init();
    BusinessProfile.init();
    DeliveryAddress.init();
});

var MyAccountPage = MyAccountPage || (function () {

    var addUploadPopupEvent = function () {
        $(document).on("click", function () {
            $('.popup-sub').fadeOut();
        });

        //$(".popup-sub").click(function () { return false; });
        $(".lm__avatar .lm__avatar__upload-btn").click(function () {
            var popupSub = $(this).parent();
            if (popupSub.has('.popup-sub')) {
                popupSub.find('.popup-sub').fadeToggle();
            }
            return false;
        });
    };

    var init = function () {
        addUploadPopupEvent();
        //Init for Profile picture
        var userParamsForUpload = getSettingsForUserUpload();
        var userUploadImageModule = new UploadImageModule(userParamsForUpload);
        userUploadImageModule.initUploadFile();
        userUploadImageModule.initDeleteFile();

        //Init for Customer picture
        var companyUploadSettings = getSettingsForCompanyUpload();
        var companyUploadImageModule = new UploadImageModule(companyUploadSettings);
        companyUploadImageModule.initUploadFile();
        companyUploadImageModule.initDeleteFile();
    };
    function getSettingsForUserUpload() {
        return {
            fileSelector: "#user-avatar__file",
            previewImgSelector: "#user-avatar",
            uploadBtnSelector: "#user-uploadBtn",
            cancelUploadBtnSelector: "#user-cancelUpload",
            linkSelector: "#user-link",
            handleUrl: "/api/profile/user-upload-avatar",
            isNeedToUpdateHeader: true,
            deleteLinkSelector: "#user-deleteBtn",
            handleDeleteUrl: "/api/profile/user-delete-avatar",
            messages: {
                confirmationDialogSelector: "#dialog-user-confirmation"
            }

        };
    }

    function getSettingsForCompanyUpload() {
        return {
            fileSelector: "#company-avatar__file",
            previewImgSelector: "#company-avatar",
            uploadBtnSelector: "#company-uploadBtn",
            cancelUploadBtnSelector: "#company-cancelUpload",
            linkSelector: "#company-link",
            handleUrl: "/api/profile/company-upload-avatar",
            deleteLinkSelector: "#company-deleteBtn",
            handleDeleteUrl: "/api/profile/company-delete-avatar",
            messages: {
                confirmationDialogSelector: "#dialog-company-confirmation"
            }

        };
    }
    return {
        init: init
    };

})();

var HandinSignInInformationPage = HandinSignInInformationPage || (function () {
    var unsaved = false;
    var formSubmitting = false;


    var init = function () {
        var x = $('#hiddenSocialNumber').val();
        if (x && x !== "") {
            $('#hiddenSocialNumber, #SocialSecurityNumber').val('************');
            $('.lm__block .lm__checkbox').show();
        }

        $('#SocialSecurityNumber').change(function () {
            unsaved = true;
        });

        window.onload = function () {
            window.addEventListener("beforeunload", function (e) {
                if (formSubmitting || !unsaved) {
                    return undefined;
                }

                var confirmationMessage = 'It looks like you have been editing something. '
                    + 'If you leave before saving, your changes will be lost.';

                (e || window.event).returnValue = confirmationMessage; //Gecko + IE
                return confirmationMessage; //Gecko + Webkit, Safari, Chrome etc.
            });
        };
    };

    var validateForm = function () {
        var x = $('#SocialSecurityNumber').removeClass('error').val();
        $('#error-message').hide();
        if (x == null || x === "") {
            $('#SocialSecurityNumber').addClass('error');
            $('#error-message').show().html("Personnummer måste fyllas i");
            $('.lm__block .lm__checkbox').hide();
            return false;
        }
        var rex = /^([0-9]{12})$|^([0-9]{10})$/;
        if (!rex.test(x)) {
            $('#SocialSecurityNumber').addClass('error');
            $('#error-message').show().html("Du måste ange 12 siffror (ÅÅÅÅMMDDNNNN)");
            $('.lm__block .lm__checkbox').hide();
            return false;
        }

        formSubmitting = true;
    };

    return {
        init: init,
        validateForm: validateForm
    };

})();

var CompanyInformation = CompanyInformation || (function () {
    var init = function () {
        $(document).ready(function () {
            if ($().validate) $('#editCompanyInformationFrom').validate({
                errorElementClass: 'error',
                errorClass: 'error-item',
                errorElement: 'span',
                rules: {
                    CompanyName: "required",
                    Address: "required",
                    ZipCode: {
                        required: true,
                        zipCode: true
                    },
                    City: "required",
                    PhoneMobile: {
                        required: true,
                        mobileSE: true
                    },
                    PhoneWork: {
                        phoneSE: true
                    },
                    Email: {
                        required: true,
                        email: true
                    }
                },
                messages: {
                    CompanyName: "Du måste ange Företag/namn",
                    Address: "Du måste ange Gatuadress",
                    ZipCode: {
                        required: "Du måste ange Postnummer",
                        zipCode: window["validationMessage"].zipCode.valid
                    },
                    City: {
                        required: "Du måste ange Ort"
                    },
                    PhoneMobile: {
                        required: window["validationMessage"].mobileSE.required,
                        mobileSE: window["validationMessage"].mobileSE.valid
                    },
                    PhoneWork: {
                        phoneSE: window["validationMessage"].phoneSE.valid
                    },
                    Email: {
                        required: "Du måste ange E-post",
                        email: "Ange en giltig e-postadress"
                    }
                },
                highlight: function (element, errorClass, validClass) {
                    $('div.errors-list').hide();
                    $('ul.errors-list').show();
                    $('li#p_' + $(element).attr('id')).show();
                    $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
                },
                unhighlight: function (element, errorClass, validClass) {
                    $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
                    $('li#p_' + $(element).attr('id')).hide();
                    if ($('ul.errors-list').find('li[id^="p"]').is(":visible") === false) {
                        $('ul.errors-list').hide();
                    }
                }
            });
        });
    };
    return {
        init: init
    };
})();

var BusinessProfile = BusinessProfile || (function () {
    var init = function () {
        $(document).ready(function () {
            if ($().validate) $('#editBusinessProfileForm').validate({
                errorElementClass: 'error',
                errorClass: 'error-item',
                errorElement: 'span',
                rules: {
                    "BusinessProfile.Name": "required"
                },
                messages: {
                    "BusinessProfile.Name": "Du måste ange Typ av verksamhet"
                },
                highlight: function (element, errorClass, validClass) {
                    $('div.errors-list').hide();
                    $('ul.errors-list').show();
                    $('li#p_' + $(element).attr('id')).show();
                    $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
                },
                unhighlight: function (element, errorClass, validClass) {
                    $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
                    $('li#p_' + $(element).attr('id')).hide();
                    if ($('ul.errors-list').find('li[id^="p"]').is(":visible") === false) {
                        $('ul.errors-list').hide();
                    }
                }
            });
        });
    };

    return {
        init: init
    };
})();

var DeliveryAddress = DeliveryAddress || function () {

    var addDeleteAddressHandler = function () {
        var $delElement = $("#del-delivery-address");
        if ($delElement.length > 0) {
            $delElement.click(function (e) {
                e.preventDefault();
                var addressNumber = $("#addressNumber").val().toString();
                var handlePageId = $("#handler-delivery-page-id").val().toString();
                var apiUrl = "/api/handle-delivery-address/delete/" + addressNumber + "/" + handlePageId;
                $.ajax({
                    dataType: "html",
                    url: apiUrl,
                    cache: false,
                    success: function (data) {
                        if (!!data && data.toString().trim() !== "") {
                            window.location = data;
                        }
                    }
                });
            });
        }
    };

    function generateSiloRow(rowIdx) {
        var siloBox = "<div class='author-inform-form__column author-inform-form__column-full silos-box gray-background'> </div>";
        var delBtn = "<a href='#' class='silos-box__delete-btn'></a>";
        var silorDesc = " <span>Silo nr*</span> <input type='text' name='Silos[" + rowIdx + "].Number' class='silos silo-number' placeholder='12' /><span>Framkomlighet* </span>";
        var siloAccessibility = " <ul class='lm__form-dropdown type-3'>" +
            "<li class='showcase' data-value='10M'>" +
            "<a href='#' id='selected-silor" + rowIdx + "'>10 meter</a>" +
            "<ul class='dropdown'>" +
            "<li data-value='10M' class='selected'><a href='#'>10 meter</a></li>" +
            "<li data-value='12M'><a href='#'>12 meter</a></li>" +
            "<li data-value='24M'><a href='#'>24 meter</a></li>" +
            "</ul>" +
            "<input type='hidden' class='form-element silo-accessibility' name='Silos[" + rowIdx + "].Accessibility' value='10M' />" +
            "</li>" +
            "</ul>";
        return $(siloBox).append(delBtn, silorDesc, siloAccessibility);
    };

    function setSelectedDropdown() {
        var selectedValue = $("#editDeliveryAddress .lm__form-dropdown .showcase").attr("data-value");
        if (!selectedValue) {
            return;
        }

        var liItems = $(".lm__form-dropdown .dropdown li");
        // reset selected state
        liItems.removeClass("selected");
        liItems.each(function () {
            var data = $(this).attr("data-value");

            if (data == selectedValue) {
                $(this).addClass("selected");
            }
        });
    }

    function addSilo() {
        $('#btn-add-silo').click(function (e) {
            var rowCount = $("div.silos-box").length;
            var silorContainer = $('.silo-container');
            var newRow = generateSiloRow(rowCount);
            silorContainer.append(newRow);
            setDropdown($(".lm__form-dropdown"), 'type-3');

            addRemoveSiloHandler();

            e.preventDefault();
        });
    }

    function addRemoveSiloHandler() {
        $(".silos-box__delete-btn").click(function (e) {
            e.preventDefault();
            $(this).closest('div.silos-box').remove();
            $('input.silo-number').each(function (idx, item) {
                $(this).attr('name', 'Silos[' + idx + '].Number');
            });
            $('input.silo-accessibility').each(function (idx, item) {
                $(this).attr('name', 'Silos[' + idx + '].Accessibility');
            });
        });
    }
    function showOrHideDialog(name) {
        var indicatorId = "#has-" + name + "-indicator";
        var $itemIndicator = $(indicatorId);
        if ($itemIndicator && $itemIndicator.length > 0) {
            var updateSuccess = $itemIndicator.val().toString().toLowerCase() === "true";
            if (updateSuccess) {
                //show success dialog
                var modalId = "#modal-" + name + ".lm__information-modal__wrapper";
                var modal = $(modalId);
                modal.removeClass("hidden");
            }
        }
    }
    function addNotificationReceiverHandler() {
        var $receiverCheckBox = $('input.notification-receiver-handler');
        if (!!$receiverCheckBox && $receiverCheckBox.length > 0) {
            $receiverCheckBox.click(function () {
                $(this).val($(this).is(":checked"));
            });
        }
    }

    function init() {
        showOrHideDialog("added");
        showOrHideDialog("updated");
        showOrHideDialog("deleted");

        if (!$("#editDeliveryAddress") || $("#editDeliveryAddress").length == 0) {
            return;
        }

        setSelectedDropdown();
        addSilo();
        addRemoveSiloHandler();
        addDeleteAddressHandler();
        addNotificationReceiverHandler();
    }
    return {
        init: init
    }

}();


$(function () {
    MachineDetailPage.init();
    MachineAddPage.Init();
    MachineBookingService.Init();
});

(function ($) {
    $.fn.imgLoad = function (callback) {
        return this.each(function () {
            if (callback) {
                if (this.complete || /*for IE 10-*/ $(this).height() > 0) {
                    callback.apply(this);
                }
                else {
                    $(this).on('load', function () {
                        callback.apply(this);
                    });
                }
            }
        });
    };
})(jQuery);

var MachineDetailPage = MachineDetailPage || (function () {
    var updated = false;

    var addUploadPopupEvent = function () {
        $('.avatar-icon').on('click', function (e) {
            e.preventDefault();
            $(this).parents('.lm__block-metadata').find('.list-action-item').toggle();
        });
    };

    var fetchTotalHoursForMachine = function () {
        var inno = $('#inno').val();
        if (inno) {
            var url = 'https://api.trackunit.com/public/GetUnit';
            $.ajax({
                url: url,
                type: "GET",
                cache: false,
                data: { token: '11df2ae9bba646ceb8be959f23445b23', format: 'json', referenceNumber: inno },
                dataType: 'jsonp',
                success: function (result) {
                    if (result && result.list && result.list.length > 0) {
                        var trackUnitInfo = result.list[0];
                        var totalHours = ((parseFloat(trackUnitInfo.preRun1) + parseFloat(trackUnitInfo.run1)) / 3600).toFixed(1);
                        $('#total_hours').show().find('.total-text').html(totalHours + ' t');
                    } else {
                        $('#generic_banner').show();
                    }
                }
            });
        } else {
            $('#generic_banner').show();
        }
    };

    var autoFitImages = function () {
        var images = $(".machine-item__thumbnail-wrapper img");

        images.each(function () {
            $(this).imgLoad(function () {
                var $this = $(this);
                var size = {
                    width: $(this).width(),
                    height: $(this).height()
                };
                if (size.height < size.width) {
                    //console.log(size.width / size.height + ', ' + $this.parent().width() / $this.parent().height());
                    if (size.width / size.height > $this.parent().width() / $this.parent().height()) {
                        $(this).removeClass('horizontal-img').addClass('vertical-img');
                    } else {
                        $(this).removeClass('vertical-img').addClass('horizontal-img');
                    }
                } else {
                    //console.log(size.height / size.width + '; ' + $this.parent().height() / $this.parent().width());
                    if (size.height / size.width > $this.parent().height() / $this.parent().width()) {
                        $(this).removeClass('vertical-img').addClass('horizontal-img');
                    } else {
                        $(this).removeClass('horizontal-img').addClass('vertical-img');
                    }
                }
            });
        });
    };

    var attachDeleteMachineEvent = function () {
        $('.remove-machine-button').click(function () {
            $('.delete-machine-alert').show();
        });
        $('#agree-remove-machine').click(function () {
            $('#form-remove-machine').submit();
        });
        $('#disagree-remove-machine').click(function () {
            $('.lm__modal-alert').hide();
        });

        var isFailed = $('#deleteFailed').val();
        if (isFailed === "True") {
            alert("Radera maskin misslyckades. Kontrollera igen!");
        }
    }

    var init = function () {
        addUploadPopupEvent();
        fetchTotalHoursForMachine();
        attachDeleteMachineEvent();
        autoFitImages();

        $(window).on('resize orientationchange', function () {
            if (!updated) {
                updated = true;
                setTimeout(function () {
                    autoFitImages();
                    updated = false;
                }, 100);
            }
        });

        $(window).trigger('resize');
        
        var machineUploadSettings = {
            fileSelector: "#machine-file",
            previewImgSelector: "#machine-picture",
            uploadBtnSelector: "#machine-uploadBtn",
            cancelUploadBtnSelector: "#machine-cancelUpload",
            linkSelector: "#machine-link",
            handleUrl: "/api/machine/machine-upload-avatar",
            isNeedToUpdateHeader: false,
            errorMessage: "Det uppstod ett fel när bilden skulle laddas upp. Var gör försök igen!",
            deleteLinkSelector: "#machine-deleteBtn",
            handleDeleteUrl: "/api/machine/machine-delete-avatar"
        };
        var userUploadImageModule = new UploadImageModule(machineUploadSettings);
        userUploadImageModule.initDataForRequest([{ 'key': 'machineId', 'value': $('#machineId').val() }]);
        userUploadImageModule.initUploadFile();
        userUploadImageModule.initDeleteFile();
    };

    return {
        init: init,
        autoFitImages: autoFitImages
    };

})();

var MachineAddPage = MachineAddPage || (function () {
    var brandDdContainerSelector = "#drop-machine-brand";
    var modelDdContainerSelector = "#drop-machine-model";

    function checkRegisterNumber() {
        var id = $('#registration').val().toUpperCase();
        if (!id || id.trim() === "") return;

        id = id.trim();
        showLoader();

        $.ajax({
            url: '/api/add-machine/check-register-number?registerNumber=' + id,
            type: "GET",
            dataType: "json",
            success: function (data) {
                hideLoader();
                if (data.id != null && data.id != '') {
                    //update serialnumber
                    $('#serial-number').val(data.serialNumber);

                    //update category
                    $('#drop-machine-category > .dropdown > li').each(function () {
                        if ($(this).attr('data-value') == data.category.Key) {
                            $('#drop-machine-category > .dropdown > li').removeClass('selected');
                            $(this).addClass('selected');
                            $(this).parents('.showcase').attr('data-value', data.category.Key);
                            $(this).parents('.showcase').find('>a').html(data.category.Name);
                            $(this).parents('.showcase').find('>input').val(data.category.Key);
                            return false;
                        }

                    });

                    //update brand and model
                    $('#drop-machine-brand > .dropdown > li').each(function () {
                        if ($(this).attr('data-value') == data.brand.Key) {
                            $('#drop-machine-brand > .dropdown > li').removeClass('selected');
                            $(this).addClass('selected');
                            $(this).parents('.showcase').attr('data-value', data.brand.Key);
                            $(this).parents('.showcase').find('>a').html(data.brand.Name);
                            $(this).parents('.showcase').find('>input').val(data.brand.Key);
                            //changeModelByBrand(data);
                            changeMachineModel(data.brand.Key, data.model.Name);
                            return false;
                        }
                    });
                }
                else {
                    $('.lm__modal-alert').show();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                hideLoader();
                alert(errorThrown.toString());
            }
        });
    }

    function changeMachineModel(selectedBrandId, modelName) {
        var brandId = selectedBrandId == undefined ? $(brandDdContainerSelector).attr('data-value') : selectedBrandId;
        var apiUrl = '/api/add-machine/get-machine-models?brandId=' + brandId;
        if (modelName != undefined) {
            apiUrl += "&modelName=" + modelName;
        }
        $.ajax({
            url: apiUrl,
            type: "GET",
            dataType: "html",
            success: function (data) {
                if (data) {
                    $(".model-dd-container").html(data);
                }
                setDropdown($(modelDdContainerSelector).parent(), 'type-3');
            }
        });
    }

    function validateFormAddMachine() {
        var category = $('#drop-machine-category');
        var brand = $(brandDdContainerSelector);
        var model = $(modelDdContainerSelector);

        var brandDesc = brand.parent().parent().find('>input');
        var modelDesc = model.parent().parent().find('>input');

        var errorList = new Array();
        if (category.attr('data-value') == "" || category.attr('data-value') == undefined) {
            errorList.push("Typ");
            category.parent().parent().find('.error-item').show();
        }
        else {
            category.parent().parent().find('.error-item').hide();
        }
        if (brand.attr('data-value') == "" || brand.attr('data-value') == undefined) {
            errorList.push("Märke");
            brand.parent().parent().find('.error-item').show();
        }
        else if (brand.attr('data-value') == "Annan" && brandDesc.val() == "") {
            errorList.push("Märke");
            brand.parent().parent().find('.error-item').show();
        }
        else {
            brand.parent().parent().find('.error-item').hide();
        }
        if (model.attr('data-value') == "" || model.attr('data-value') == undefined) {
            errorList.push("Modell");
            model.parent().parent().find('.error-item').show();
        }
        else if (model.attr('data-value') == "Annan" && modelDesc.val() == "") {
            errorList.push("Modell");
            model.parent().parent().find('.error-item').show();
        }
        else {
            model.parent().parent().find('.error-item').hide();
        }
        var errorListBox = $('#machine-add-error-list');
        if (errorList.length > 0) {
            errorListBox.show();
            errorListBox.html('');
            errorListBox.append(
                $('<li></li>').addClass('errors-list__header').append('Du måste ange')
                );
            $.each(errorList, function (i, error) {
                errorListBox.append(
                    $('<li></li>').append(error)
                    );
            });
            return false;
        }
        else {
            errorListBox.hide();
            return true;
        }
    }

    function hideLoader() {
        $('.loader-wrapper').hide();
        $('.loader-wrapper').parent().removeClass('disabled');
    }

    function showLoader() {
        $('.loader-wrapper').show();
        $('.loader-wrapper').parent().addClass('disabled');
    }

    var addBrandHandler = function () {
        $("#drop-machine-brand .dropdown li a").each(function (idx, item) {
            var $item = $(item);
            $item.click(function () {
                if ($item.parent().data('value') === 'Annan') {
                    $item.parents('.branch-block').find('input[type="text"]').show();
                    $item.parents('ul.lm__form-dropdown.branch-dropdown').hide();

                    var $modelDdContainer = $(modelDdContainerSelector);
                    $modelDdContainer.parents('.model-block').find('input[type="text"]').show();
                    $modelDdContainer.attr('data-value', 'Annan');
                    $modelDdContainer.find('>input[type="hidden"]').val('Annan');
                    $modelDdContainer.parents('ul.lm__form-dropdown.model-dropdown').hide();
                }
                else {
                    var brandId = $item.attr('data-value');
                    changeMachineModel(brandId);
                }
            });
        });
    }

    var addCategoryHandler = function () {
        $("#drop-machine-category .dropdown li a").each(function (idx, item) {
            var $item = $(item);
            $item.click(function () {
                var selectedKey = $item.parents(".showcase").data('value');
                if (selectedKey === "Other") {
                    // incase choosing other category, show textbox to input value
                    $item.parents('.category-block').find('input[type="text"]').show();
                    $item.parents('ul.lm__form-dropdown').hide();
                    $item.parents('.showcase').find("input[name=machineCategory").val("");
                }
            });
        });
    }

    var searchByRegNoHandler = function () {
        $('#machine-registration-button').click(function () {
            checkRegisterNumber();
        });
    }

    var submitAddingFormHandler = function () {
        $('#form-adding-machine').submit(function () {
            return validateFormAddMachine();
        });
    }

    var init = function () {
        addBrandHandler();
        addCategoryHandler();
        searchByRegNoHandler();
        submitAddingFormHandler();
    };
    return {
        Init: init
    }
})();

var MachineBookingService = MachineBookingService || (function () {

    var dataMachines = null;

    function setDataMachines(data) {
        dataMachines = data;
    }

    function setOnclickMachineList(obj, data) {
        $(obj).find('p > a').click(function (e) {
            if (data != null && data.length > 0) {
                var machineId = $(this).parent().find('input[type="hidden"]').val();
                fillDetailMachine(data, machineId);
            }
            e.preventDefault();
        });
    }

    function getMachinesByCategory() {
        var key = $('#book-service-category').attr('data-value');
        $.ajax({
            url: '/api/book-service/get-machine-by-category?categoryKey=' + key,
            type: "GET",
            dataType: "json",
            success: function (data) {
                $('#book-service-loading').hide();
                $.each(data, function (i, machines) {
                    if (machines.length > 6) {
                        $('#book-machine-list-2').show();
                        $('#book-machine-list-1').hide();
                        var dropdown = $('#book-machine-list-2 > .address-sections > select');
                        dropdown.html('');
                        $.each(machines, function (j, machine) {
                            dropdown.append(
                                $('<option></option>').attr('value', machine.id).append(machine.modelName)
                            );
                        });
                        setDataMachines(machines);
                    }
                    else if (machines.length > 0 && machines.length <= 6) {
                        $('#book-machine-list-2').hide();
                        $('#book-machine-list-1').show();
                        var listSelectMachine = $('#book-machine-list-1 > .address-sections');
                        listSelectMachine.html('');
                        listSelectMachine.append($('<h3></h3>').addClass('lm__bold-title').append('Mina maskiner'));
                        $.each(machines, function (j, machine) {
                            listSelectMachine.append($('<p></p>').append(
                                $('<a></a>').addClass('lm__link').attr('href', '#').append(machine.modelName),
                                $('<input></input>').attr('type', 'hidden').val(machine.id)
                                ));
                        });
                        setOnclickMachineList(listSelectMachine, machines);
                    }
                    else {
                        $('#book-machine-list-2').hide();
                        $('#book-machine-list-1').hide();
                    }
                });
            },
        });
    }

    function getMachineById(listMachine, machineId) {
        var result = null;
        $.each(listMachine, function (i, machine) {
            if (machine.id.toString() == machineId.toString()) {
                result = machine;
            }
        });
        return result;
    }

    function fillDetailMachine(listMachine, machineId) {
        var machine = getMachineById(listMachine, machineId);
        var blockDetailMachine = $('#book-service-detail-machine').parent();
        if (machine != null) {
            blockDetailMachine.show();
            $('#book-service-detail-machine').html('');
            $('#book-service-detail-machine').append(
                $('<h2></h2>').addClass('heading-title-2').append('Vald maskin')
                );
            $('#book-service-detail-machine').append(
                $('<h3></h3>').addClass('lm__bold-title').append(machine.modelName)
                );

            $('#book-service-detail-machine').append(
                $('<p></p>').append(
                    $('<span></span>').addClass('lm__bold-title').append('Serienr'),
                    machine.serialNumber
                    )
                );
            $('#book-service-detail-machine').append(
                $('<p></p>').append(
                    $('<span></span>').addClass('lm__bold-title').append('Regnr'),
                    machine.registerNumber
                    )
                );
            blockDetailMachine.find("p > input[name=BrandAndModel]").val(machine.modelName);
            blockDetailMachine.find("p > input[name=SerialNumber]").val(machine.serialNumber);
            blockDetailMachine.find("p > input[name=RegisterNumber]").val(machine.registerNumber);
        }
        else {
            blockDetailMachine.hide();
            blockDetailMachine.find("p > input[name=BrandAndModel]").val('');
            blockDetailMachine.find("p > input[name=SerialNumber]").val('');
            blockDetailMachine.find("p > input[name=RegisterNumber]").val('');
        }
    }

    function bookingServiceSendFaild() {
        var result = $('#booking-service-send-faild').val();
        if (result != "" && result != undefined) {
            alert(result);
        }
    }

    var addMaskinTypeHandler = function () {
        // for choose Maskin Type in Book Service
        $(".maskin-type .showcase .dropdown li a").click(function () {
            $('#book-service-detail-machine').parent().hide();
            $('#book-machine-list-1').hide();
            $('#book-machine-list-2').hide();
            $('#book-service-loading').show();
            getMachinesByCategory();
        });
    }

    var addAddressHandler = function () {
        $("#book-machine-list-2 > .address-sections > select").change(function () {
            if (dataMachines != null && dataMachines.length > 0) {
                fillDetailMachine(dataMachines, $(this).val());
            }
        });
    }

    var addBookServiceFailedHandler = function () {
        //alert booking service send faild
        return bookingServiceSendFaild();
    }

    var init = function () {
        addMaskinTypeHandler();
        addAddressHandler();
        addBookServiceFailedHandler();
    }

    return {
        Init: init
    }
})();

$(function () {

    var MyCompany = function () {

        var accordionLinks = $(".lm__accordion-wrapper .lm__accordion-link");
        var accordionContents = $(".lm__accordion-wrapper .lm__accordion-content");

        var addAccordionEvent = function () {
            accordionLinks.each(function () {
                $(this).click(function () {
                    if (!$(this).hasClass('active')) {
                        accordionContents.slideUp('fast', function () {
                            $(this).prev().removeClass("active");
                        });

                        $(this).next().slideDown('fast', function () {
                            $(this).prev().addClass("active");
                        });
                    } else {
                        $(this).next().slideUp('fast', function () {
                            $(this).prev().removeClass("active");
                        });
                    }
                });
            });
        };

        return {
            init: function () {
                addAccordionEvent();
                return false;
            }
        }
    };

    $(document).ready(function () {
        var myCom = new MyCompany();
        myCom.init();
    });

});
$(function () {

    var MyAccount = function () {

        var userTypeRadios = $(".choose-user-type .lm__radio input[name='user-type']");
        var subUserTypeMenu = $(".user-type__sub-types");
        var subType = $(".sub-type .lm__checkbox input[type='checkbox']");

        var showSubOptions = function (ele) {
            if (ele.is(":checked")) {
                var subUserType = ele.attr("id");

                subUserTypeMenu.hide();
                subUserTypeMenu.each(function () {
                    if ($(this).attr("data-parent") == subUserType) {
                        $(this).show();
                    }
                });
            }
        };

        var membershipChoosen = function () {

        }

                //console.log(endPoint);
        return {
            init: function () {
                return false;
            }
        }
    };

    $(document).ready(function () {
        var myAcc = new MyAccount();
        myAcc.init();
    });

});
$(function () {

    var CustomerIDPage = function () {
        var parentCheckbox = $(".customer-id-block .lm__block-box .parent-checkbox .lm__checkbox input");
        var childCheckboxes = parentCheckbox.parents(".lm__block-box").find(".sub-checkbox .lm__checkbox input");
        var tooltipLink = $(".link-to-open-popup");
        var tooltipMask = $(".lm__information-modal__wrapper .mask");

        var addCheckboxEvent = function () {
            if (parentCheckbox) {
                parentCheckbox.change(function () {
                    var _that = $(this);
                    var childCheckboxesWrapper = _that.parents(".lm__block-box").find(".sub-checkbox");
                    var childCheckboxes = childCheckboxesWrapper.find(".lm__radio input");

                    if (childCheckboxesWrapper && childCheckboxes) {
                        if (_that.is(":checked")) {
                            childCheckboxes.parent().removeClass('sub-hidden');
                            $(childCheckboxes[0]).prop('checked', true);
                        } else {
                            childCheckboxes.parent().addClass('sub-hidden');
                           childCheckboxes.prop('checked', false);
                        }
                    }

                    return false;
                });
            }
        };

        var tooltipEvent = function () {
            if (tooltipLink && tooltipMask) {
                tooltipLink.click(function (e) {
                    e.preventDefault();
                    var tooltipPanel = $(this).parent().next();
                    if (tooltipPanel) tooltipPanel.removeClass("hidden");
                    return false;
                });

                tooltipMask.click(function () {
                    $(this).parent().addClass("hidden");
                });
            }
        };

        return {
            init: function () {
                addCheckboxEvent();
                tooltipEvent();
                return false;
            }
        }
    };

    $(document).ready(function () {
        var customerID = new CustomerIDPage();
        customerID.init();
    });

});
$(function () {
    var TermOfUse = function () {
        function registerHandler() {
            if ($("#term-of-use-no").length === 0 && $("#term-of-use-yes").length === 0) {
                return;
            }

            $("#term-of-use-no").click(function () {
                var firstRefectDialog = $(".lm__main-content").find("#dialog-first-reject");
                firstRefectDialog.fadeIn();

                $(".back-to-term", firstRefectDialog).click(function () {
                    firstRefectDialog.fadeOut();
                });

                $(".reject", firstRefectDialog).click(function () {
                    var secondRejectDialog = $(".lm__main-content").find("#dialog-reconfirm-reject");
                    secondRejectDialog.fadeIn();
                    firstRefectDialog.fadeOut();

                    $(".back-to-term", secondRejectDialog).click(function () {
                        secondRejectDialog.fadeOut();
                    });

                    $(".reject", secondRejectDialog).click(function () {
                        secondRejectDialog.fadeOut();
                        $("#rejectForm").submit();
                    });
                });
            });
        }

        return {
            init: function () {
                registerHandler();
            }
        }
    }

    $(document).ready(function () {
        var termOfUse = new TermOfUse();
        termOfUse.init();
    });
});
if (jQuery.validator) {

    function isValidate(selector) {
        var isValid = $(selector).val() !== "True";
        if (!isValid) {
            $(selector).val('False');
        }
        return isValid;
    }

    jQuery.validator.addMethod("CustomerNumberNotMatch", function (value, element, selector) {
        return isValidate(selector);
    }, "Kundnumret du har angett finns inte registrerat hos Lantmännen för det organisationsnummer du har angett.");
    jQuery.validator.addMethod("CustomerNumberNotExist", function (value, element, selector) {
        return isValidate(selector);
    }, 'Kundnumret finns inte registrerat hos Lantmännen.');
    jQuery.validator.addMethod("CustomerNumberActivated", function (value, element, selector) {
        return isValidate(selector);
    }, 'Kundnumret och organisationsnumret  du har angett är redan aktiverat för ett konto i LM2.');
    jQuery.validator.addMethod("OrganisationNumberNotMatch", function (value, element, selector) {
        return isValidate(selector);
    }, 'Organisationsnumret du har angett stämmer inte överens med kundnumret du har angett.');
    jQuery.validator.addMethod("OrganisationNumberActivated", function (value, element, selector) {
        return isValidate(selector);
    }, 'Organisationsnumret och kundnumret du har angett är redan aktiverat för ett konto i LM2.');
}

jQuery(function () {
    ActiveFarmdayCustomer.init();
    ActiveFarmdayCustomer.initEvent();
    ActiveFarmdayCustomer.initValidateTermOfUseForm();
});

var ActiveFarmdayCustomer = ActiveFarmdayCustomer || (function () {
    var init = function() {
        if ($().validate) $('#verifyCustomer').validate({
            errorElementClass: 'error',
            errorClass: 'error-item',
            errorElement: 'span',
            rules: {
                CustomerNumber: {
                    required: true,
                    CustomerNumberNotMatch: '#CustomerNumberNotMatch',
                    CustomerNumberNotExist: '#CustomerNumberNotExist',
                    CustomerNumberActivated: '#CustomerNumberActivated'
                },
                OrganisationNumber: {
                    required: true,
                    regex: /^\d{6}[-]{0,1}\d{4}$/,
                    OrganisationNumberNotMatch: '#OrganisationNumberNotMatch',
                    OrganisationNumberActivated: '#OrganisationNumberActivated'
                }
            },
            messages: {
                CustomerNumber: {
                    required: "Du måste ange Kundnumret"
                },
                OrganisationNumber: {
                    required: "Du måste ange Organisationsnumret",
                    regex: "Organisationsnumret du har angett är av fel typ."
                }
            },
            highlight: function (element, errorClass, validClass) {
                $('div.errors-list').hide();
                $('ul.errors-list').show();
                $('li#p_' + $(element).attr('id')).show();
                $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
                $('li#p_' + $(element).attr('id')).hide();
                if ($('ul.errors-list').find('li[id^="p"]').is(":visible") === false) {
                    $('ul.errors-list').hide();
                }
            },
            submitHandler: function(form) {
                $('#loader').show();
                $('#loader').parent().addClass('disabled');
                form.submit();
            }
        });

        if ($().validate) {
            var isInvalid = $('#CustomerNumberNotMatch').val() === "True" ||
                $('#CustomerNumberNotExist').val() === "True" ||
                $('#CustomerNumberActivated').val() === "True" ||
                $('#OrganisationNumberNotMatch').val() === "True" ||
                $('#OrganisationNumberActivated').val() === "True";
            if (isInvalid) {
                $("#verifyCustomer").valid();
            }
        }
    }

    var initEvent = function () {
        $('#expandTermOfUse').click(function () {
            $(this).toggleClass('expanded').next().toggle();
        });

        $(document).on('change', '#acceptTerm', function() {
            if ($(this).is(':checked')) {
                $('.success-icon').show().parent().next().html('Du har godkänt <a href="#" class="lm__link">användningsvillkoren</a> för LM<sup>2</sup>.');
                $('#submitBtn').removeClass('disabled-btn');
                $('#expandTermOfUse').toggleClass('expanded').next().toggle();
            } else {
                $('.success-icon').hide().parent().next().html('Du måste läsa och godkänna <a href="#" class="lm__link">användningsvillkoren</a> för att registrera ett LM<sup>2</sup>-konto');;
                $('#submitBtn').addClass('disabled-btn');
            }

        });
    }


    var initValidateTermOfUseForm = function() {
        if ($().validate)
            $('#registerCustomer').validate({
                errorElementClass: 'error',
                errorClass: 'error-item',
                errorElement: 'span',
                rules: {
                    ContractApplication: {
                        required: true
                    },
                    CopyOfIDDocuments: {
                        required: true
                    },
                    RegistrationCertificate: {
                        required: true
                    },
                    FirstName: {
                        required: true
                    },
                    SurName: {
                        required: true
                    },
                    Telephone: {
                        regex: /^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$/
                    },
                    Mobile: {
                        required: true,
                        regex: /^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$/
                    },
                    PersonNumber: {
                        required: true,
                        regex: /^[0-9]{12}$/
                    }
                },
                messages: {
                    ContractApplication: {
                        required: "Du måste bifoga ansökan om avtal för e-tjänster"
                    },
                    CopyOfIDDocuments: {
                        required: "Du måste bifoga Kopia på ID-handlingar"
                    },
                    RegistrationCertificate: {
                        required: "Du måste bifoga Registreringsbevis för ditt företag"
                    },
                    FirstName: {
                        required: "Förnamn är obligatorisk"
                    },
                    SurName: {
                        required: "Efternamn är obligatoriskt"
                    },
                    Telephone: {
                        regex: "Ange en giltig telefonnummer"
                    },
                    Mobile: {
                        required: "Mobilnr är obligatoriskt",
                        regex: "Ange ett giltigt mobilnummer"
                    },
                    PersonNumber: {
                        required: "Personnr är obligatoriskt",
                        regex: "Personnr måste bestå av 12 siffror"
                    }
                },
                highlight: function (element, errorClass, validClass) {
                    $('div.errors-list').hide();
                    $('ul.errors-list').show();
                    $('li#p_' + $(element).attr('id')).show();
                    var typeElement = $(element).attr('type');
                    if (typeElement === 'file') {
                        $(element).parent().addClass(this.settings.errorElementClass).removeClass(errorClass);
                    } else {
                        $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
                    }

                },
                unhighlight: function (element, errorClass, validClass) {
                    var typeElement = $(element).attr('type');
                    if (typeElement === 'file') {
                        $(element).parent().removeClass(this.settings.errorElementClass).removeClass(errorClass);
                    } else {
                        $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
                    }

                    $('li#p_' + $(element).attr('id')).hide();
                    if ($('ul.errors-list').find('li[id^="p"]').is(":visible") === false) {
                        $('ul.errors-list').hide();
                    }
                },
                errorPlacement: function (error, element) {
                    var typeElement = element.attr('type');
                    if (typeElement === 'file') {
                        element.parent().after(error);
                    } else {
                        element.after(error);
                    }

                }
            });
    }

    return {
        init: init,
        initEvent: initEvent,
        initValidateTermOfUseForm: initValidateTermOfUseForm
    };
})();
if (jQuery.validator) {
    if (jQuery.validator.regex == undefined) {
        jQuery.validator.addMethod("regex", function (value, element, regexpr) {
            return this.optional(element) || regexpr.test(value);
        }, 'Please enter a valid value.');
    }
}

var ContactForm = ContactForm || function () {
    var formValidator = undefined;

    function addValidation() {
        if ($('#ContactForm').length === 0) {
            return;
        }
        $(document).ready(function () {
            var rules = {
                subject: "required",
                message: "required"
            };

            var messages = {
                subject: "Du måste ange Ämne",
                message: "Du måste ange Meddelande"
            }
            if ($('#loginFlag').val().toString().toLowerCase !== "true") {
                rules.name = "required";
                rules.email = {
                    required: true,
                    email: true
                }
                rules.customerNumber = {
                    required: true,
                    regex: /^(\d+)/
                };

                messages.name = "Du måste ange Ditt namn";
                messages.email = {
                    required: "Du måste ange E-post",
                    email: "Ange en giltig e-postadress"
                };
                messages.customerNumber = {
                    required: "Du måste ange Kundnummer",
                    regex: "Ange en giltig Kundnummer"
                }
            }
            if ($().validate) {
                formValidator = $('#ContactForm').validate({
                    ignore: [],
                    errorElementClass: 'error',
                    errorClass: 'error-item',
                    errorElement: 'span',
                    rules: rules,
                    messages: messages,
                    highlight: function (element, errorClass, validClass) {
                        //console.log("jquery validation");
                        $('ul.errors-list').show();
                        $('li#li_' + $(element).attr('id')).show();
                        $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
                    },
                    unhighlight: function (element, errorClass, validClass) {
                        $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
                        $('li#li_' + $(element).attr('id')).hide();
                        if ($('ul.errors-list').find('li[id^="li"]').is(":visible") === false) {
                            $('ul.errors-list').hide();
                        }
                    }
                });

                $("input[type=reset]").click(function() {
                    if (formValidator) {
                        formValidator.resetForm();

                        // hide all errors
                        $('ul.errors-list').hide();
                        $('.error').each(function() {
                            $(this).removeClass('error');
                        });
                    }
                });
            }
        });
    }


    function init() {
        addValidation();
    }
    return {
        init: init
    }

}();

$(function () {
    ContactForm.init();
});
$(function () {

    var UpcomingDelivery = function () {
        var carousel = $(".lm__carousel-block");
        //var carouselItem = carousel.find(".lm__carousel__content .lm__carousel__item .lm__blue-btn");
        var carouselItem = carousel.find(".lm__carousel__item a").not(".callendar");
        var $itemPopup = $("#upcoming-detail");
        var agreementSidebar = $(".lm__block.agreement");

        var showInformation = function () {
            carouselItem.on('click', function (e) {
                e.preventDefault();
                var $this = $(this);

                var deliveryObj = getDeliveryInfor($this);

                $.ajax({
                    type: "post",
                    data: deliveryObj,
                    dataType: "html",
                    url: "/api/upcomming-deliveries/get-detail",
                    success: function (data) {
                        $itemPopup.html(data);
                        $itemPopup = $("#upcoming-detail");
                        $itemPopup.fadeIn();
                    }
                });

                return false;
            });
        };

        function getDeliveryInfor($carouselItem) {
            //var $inforElement = $carouselItem.parent().parents();
            var $inforElement = $carouselItem.parent();
            var isDeliveryFromCustomer = $carouselItem.closest("section").attr("data-isFromCustomer");
            return {
                orderNo: $inforElement.attr("data-orderNo"),
                orderLine: $inforElement.attr("data-orderLine"),
                itemName: $inforElement.attr("data-itemName"),
                orderQuantity: $inforElement.attr("data-orderQuantity"),
                warehouse: $inforElement.attr("data-warehouse"),
                container: $inforElement.attr("data-container"),
                planedDeliveryDate: $inforElement.attr("data-planedDate"),
                isFromCustomer: isDeliveryFromCustomer
            }
        }

        // sidebars
        var toggleSidebar = function () {
            if (agreementSidebar.length > 0) agreementSidebar.each(function () {
                var items = $(this).find(".lm__news-item");
                items.each(function () {
                    var $that = $(this);
                    var toggleLink = $that.find(">a");

                    if ($that.hasClass('open')) {
                        $that.find('.detail-information').slideDown('fast');
                    }


                    toggleLink.on('click', function () {
                        var $this = $(this);
                        var parentItem = $this.parent();
                        var isOpen = parentItem.hasClass('open');
                        var detailInformation = $this.next();

                        items.removeClass('open').find('.detail-information').slideUp('fast');
                        if (isOpen) {
                            parentItem.removeClass('open');
                            detailInformation.slideUp('fast');
                        } else {
                            parentItem.addClass('open');
                            detailInformation.slideDown('fast');
                        }
                        return false;
                    });
                });
            });
        };

        return {
            showInformation: function () {
                showInformation();
            },
            toggleSidebar: function () {
                toggleSidebar();
            }
        }
    };

    $(document).ready(function () {
        // stick carousel
        if ($.fn.slick) $('.lm__carousel-block .lm__carousel__content').slick({
            dots: true,
            infinite: false,
            speed: 300,
            slidesToShow: 3,
            slidesToScroll: 3,
            responsive: [
            //{
            //    breakpoint: 1366,
            //    settings: {
            //        slidesToShow: 3,
            //        slidesToScroll: 3
            //    }
            //},
            {
                breakpoint: 1100,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    variableWidth: true,
                    centerMode: true,
                    centerPadding: '40px',
                    infinite: true
                }
            },
            {
                breakpoint: 640,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    dots: false,
                    arrows: false,
                    variableWidth: true,
                    centerMode: true,
                    centerPadding: '40px',
                    //infinite: true
                }
            }
            ]
        });

        // init event for item in upcoming delivery
        var upcomingDelivery = new UpcomingDelivery();
        upcomingDelivery.showInformation();
        upcomingDelivery.toggleSidebar();
    });
});
$(function () {
    if ($.fn.datepicker) {
        $(".datepicker-blank-default").datepicker({
            onSelect: function(newText) {
                if ($(this).is("#selectedDate")) {
                    if ($(this).data("previous") !== newText) {
                        var timeBooking = new TimeBookingPage();
                        timeBooking.searchItems("", $("#resourceDrp").attr("data-value"));
                        timeBooking.searchAvalibleSlotAction();
                    }

                }
                var error = $("#error-fromDate");
                if ($(this).is("#fromDateTxt")) {
                    var toDateValue = $("#toDateTxt").val();
                    if (validateDateValue(newText, toDateValue) === 1) {
                        error.show();
                    } else if (validateDateValue(newText, toDateValue) === -1) {
                        $("#toDateTxt").val(newText);
                        error.hide();
                    } else {
                        error.hide();
                    }
                }
                if ($(this).is("#toDateTxt")) {
                    var fromDateValue = $("#fromDateTxt").val();
                    if (validateDateValue(fromDateValue, newText) === 1) {
                        error.show();
                    } else {
                        error.hide();
                    }
                }
            },
            monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec"],
            dayNamesMin: ["Sö", "Må", "Ti", "On", "To", "Fr", "Lö"],
            firstDay: 1,
            dateFormat: "yy-mm-dd",
            beforeShow: function () {
                setTimeout(function () {
                    $('.ui-datepicker').css('z-index', 5);
                }, 0);
                if ($(this).is("#toDateTxt")) {
                    var fromDateValue = $("#fromDateTxt").val();
                    var toDateValue = $("#toDateTxt").val();
                    $(this).datepicker('option', 'minDate', new Date(fromDateValue));
                    $("#toDateTxt").val(toDateValue);
                }
                if ($(this).is("#selectedDate")) {
                    $(this).datepicker('option', 'minDate', new Date());
                }
            }
        }).on("change", function () {
            var $this = $(this);
            var validDate = !/Invalid|NaN/.test(new Date($this.val()).toString());
            var validDateRegex = /^\d\d\d\d-(0?[1-9]|1[0-2])-(0?[1-9]|[12][0-9]|3[01])$/ig.test($this.val());

            if (!validDateRegex || !validDate) {
                //$this.addClass("error").next().show();
                $this.next().show();
            } else {
                //$this.removeClass("error").next().hide();
                $this.next().hide();
            }
        });

        $(".datepicker-blank-dialog").datepicker({
            onSelect: function (newText) {
                if ($(this).is("#selectedDate")) {
                    if ($(this).data("previous") !== newText) {
                        var timeBooking = new TimeBookingPage();
                        timeBooking.searchItems("", $("#resourceDrp").attr("data-value"));
                        timeBooking.searchAvalibleSlotAction();
                    }

                }
                var error = $("#error-fromDate");
                if ($(this).is("#fromDateTxt")) {
                    var toDateValue = $("#toDateTxt").val();
                    if (validateDateValue(newText, toDateValue) === 1) {
                        error.show();
                    } else if (validateDateValue(newText, toDateValue) === -1) {
                        $("#toDateTxt").val(newText);
                        error.hide();
                    } else {
                        error.hide();
                    }
                }
                if ($(this).is("#toDateTxt")) {
                    var fromDateValue = $("#fromDateTxt").val();
                    if (validateDateValue(fromDateValue, newText) === 1) {
                        error.show();
                    } else {
                        error.hide();
                    }
                }
            },
            monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec"],
            dayNamesMin: ["Sö", "Må", "Ti", "On", "To", "Fr", "Lö"],
            firstDay: 1,
            dateFormat: "yy-mm-dd",
            beforeShow: function () {
                setTimeout(function () {
                    $('.ui-datepicker').css('z-index', 9);
                }, 0);
                if ($(this).is("#toDateTxt")) {
                    var fromDateValue = $("#fromDateTxt").val();
                    var toDateValue = $("#toDateTxt").val();
                    $(this).datepicker('option', 'minDate', new Date(fromDateValue));
                    $("#toDateTxt").val(toDateValue);
                }
                if ($(this).is("#selectedDate")) {
                    $(this).datepicker('option', 'minDate', new Date());
                }
            }
        }).on("change", function () {
            var $this = $(this);
            var validDate = !/Invalid|NaN/.test(new Date($this.val()).toString());
            var validDateRegex = /^\d\d\d\d-(0?[1-9]|1[0-2])-(0?[1-9]|[12][0-9]|3[01])$/ig.test($this.val());

            if (!validDateRegex || !validDate) {
                //$this.addClass("error").next().show();
                $this.next().show();
            } else {
                //$this.removeClass("error").next().hide();
                $this.next().hide();
            }
        });
    }

    $(document).ready(function () {
        $(".expand-info").click(function () {
            var currentRow = $($(this).parent().parent());
            var hiddenRow = currentRow.next();
            hiddenRow.slideToggle(100);
            if ($(this).hasClass("expand-icon-plus")) {
                $(this).removeClass("expand-icon-plus");
                $(this).addClass("expand-icon-minus");
            }
            else {
                $(this).removeClass("expand-icon-minus");
                $(this).addClass("expand-icon-plus");
            }
        });
        TransportTypeChangeEvent();
    });
    $(".showDialog").click(function (e) {
        getAnnoncements($(this));
        e.preventDefault();
    });
    $(".table-transport > tbody > tr > td > a").click(function (e) {
        submitForm($(this).html(), $(this).attr('data-value'));
        e.preventDefault();
    });

    $(".search-customerNr").click(function(e) {
        submitForm($(this).attr('data-customerNr'), $(this).attr('data-value'));
        e.preventDefault();
    });

    $(document).on("click", ".datepicker-container", function () {
        console.dir("parent clicked");
        $(this).find("input").focus();
    });
});

var validateDateValue = function (fromData, toDate) {
    var myFromDate = new Date(fromData);
    var myToDate = new Date(toDate);
    var timeOfSevenDay = (24 * 60 * 60 * 1000) * 7;
    if (myToDate.getTime() - myFromDate.getTime() > timeOfSevenDay) {
        return 1;
    }
    else if (myToDate.getTime() - myFromDate.getTime() < 0) {
        return -1;
    }
    return 0;
};

var setToDateByFromDate = function(fromDateValue, toDateValue) {
    var myFromDate = new Date(fromDateValue);
    var myToDate = new Date(toDateValue);
    if (myFromDate.getTime() > myToDate.getTime()) {
        return 
    }
}
function submitForm(search, category) {
    var searchVal = search;
    var catVal = category;
    $("input[name= 'SearchText']").val(searchVal);
    $("#search-transport-type > ul > li").each(function() {
        if ($(this).attr("data-value") === catVal) {
            $("input[name= 'Category']").val(catVal);
        }
    });
    $("#form-search-transport").submit();
}

function getAnnoncements(row) {
    var orderRowId = $(row).parents(".lm__collapse-row").prev().find("td > a").html();
    var annoncementTable = $(".tblAnnoncements tbody");
    var modal = $(".lm__information-modal__wrapper");

    $.ajax({
        type: "POST",
        url: "/api/search-transport/get-annoncements",
        contentType: "application/json; charset=utf-8",
        data: "{'orderRowId':'" + orderRowId + "'}",

        success: function (data) {
            annoncementTable.html("");
            annoncementTable.html(data);
            modal.removeClass("hidden");
        },
        error: function () {
            annoncementTable.html("");
            modal.removeClass("hidden");
        }
    });
}

var TransportTypeChangeEvent = function () {
    $("#search-transport-type > ul > li > a").click(function () {
        var dataValueType = $(this).parent().attr("data-value");
        var placeholderData = "Ange " + dataValueType.toLowerCase();
        $("input[name='Category']").val(dataValueType);
        $("input[name= 'SearchText']").attr('placeholder', placeholderData);
        var inputFromDate = $("input[name= 'FromDate']");
        var inputToDate = $("input[name= 'ToDate']");
        if (dataValueType === "Kundnummer" || dataValueType === "Lastbil") {
            inputFromDate.prop("disabled", false);
            inputToDate.prop("disabled", false);
            if (inputFromDate.parent().hasClass("disabled")) {
                inputFromDate.parent().removeClass("disabled");
            }
            if (inputToDate.parent().hasClass("disabled")) {
                inputToDate.parent().removeClass("disabled");
            }
        } else {
            inputFromDate.prop("disabled", true);
            inputToDate.prop("disabled", true);
            inputFromDate.val("");
            inputToDate.val("");
            inputFromDate.parent().addClass("disabled");
            inputToDate.parent().addClass("disabled");

        }
    });
}

window.validation = {
    phoneSE: function (phoneNumber) {
        phoneNumber = phoneNumber.replace(/\s+/g, "");
        return phoneNumber.match(/^(\+46)\d{6,}$/);
    },

    mobileSE: function (phoneNumber) {
        phoneNumber = phoneNumber.replace(/\s+/g, "");
        return phoneNumber.match(/^(\+46)\d{9}$/);
    },

    zipCode: function (zipCode) {
        zipCode = zipCode.replace(/\s+/g, "");
        return zipCode.match(/^(\d{5}$)/);
    },

    personNumber: function (personNumber) {
        personNumber = personNumber.replace(/\s+/g, "");
        return personNumber.match(/^\d{12}$/);
    },
    swedishDate:function(swedishDate) {
        var swedishDateRegex = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})$/;
        return swedishDate.match(swedishDateRegex) && ((new Date(swedishDate)) !="Invalid Date");
    }
};

window.validationMessage = {
    firstName: {
        required: "Du måste ange Förnamn"
    },
    lastName: {
        required: "Du måste ange Efternamn"
    },
    phoneSE: {
        valid: "Ange ett korrekt telefonnummer"
    },
    mobileSE: {
        required: "Du måste ange Mobilnr",
        valid: "Ange ett korrekt mobilnummer"
    },
    zipCode: {
        required: "Postnumret ska bestå av 5 siffror",
        valid: "Postnumret ska bestå av 5 siffror"
    },
    email: {
        required: "Du måste ange E-post",
        valid: "Ange en giltig e-postadress"
    },
    personNumber: {
        valid: "Personnumret ska bestå av 12 siffror"
    },
    swedishDate: {
        required:"Du måste ange ett giltigt datum",
        valid: "Du måste ange ett giltigt datum"
    }
};

$(function () {
    if (!jQuery.validator) {
        return;
    }

    $.validator.addMethod("phoneSE",
        function (phoneNumber, element) {
            return this.optional(element) || window.validation.phoneSE(phoneNumber);
        },
        window.validationMessage.phoneSE.valid);

    $.validator.addMethod("mobileSE",
        function (phoneNumber, element) {
            return window.validation.mobileSE(phoneNumber);
        },
        window.validationMessage.mobileSE.valid);

    $.validator.addMethod("zipCode",
        function (zipCode, element) {
            return this.optional(element) || window.validation.zipCode(zipCode);
        },
        window.validationMessage.zipCode.valid);

    $.validator.addMethod("personNumber",
        function (pn, element) {
            return this.optional(element) || window.validation.personNumber(pn);
        },
        window.validationMessage.personNumber.valid);

    $.validator.addMethod("swedishDate",
       function (pn, element) {
           return window.validation.swedishDate(pn);
       },
       window.validationMessage.swedishDate.valid);
});

jQuery(function () {
    DeliveryAssuranceModule.init();
});

var DeliveryAssuranceModule = DeliveryAssuranceModule || (function () {
    var reqDate, startDate, endDate, harvestPeriodStart, harvestPeriodEnd;
    var existDepaAvtal, choosenSpontanAvtal;
    var isInternal = 0;
    var enabledWarehouse = 0;

    var getCalculateObject = function () {
        return {
            supplier: $('#dialog_CustomerNumber').val(),
            lorryType: $('#dialog_LorryType').val(),
            deliveryAddressId: $('#dialog_DeliveryAddress').val(),
            quantity: $('#dialog_Quantity').val(),
            deliveryDate: $('#dialog_DeliveryDate').val(),
            itemId: $('#dialog_Article').val()
        };
    }

    var setSelectedForCustomSelectBox = function (targeSelector, sourceSelector) {
        var selectedValue = $(sourceSelector).val();
        $(targeSelector).prev().find('li').each(function (index, item) {
            var matchValue = $(item).attr('data-value');
            if (matchValue === selectedValue) {
                $(item).find('a').trigger('click');
            }
        });
    }

    var fillCalculateForm = function () {
        $('#dialog_Quantity').val($('#Quantity').val());
        $('#dialog_DeliveryDate').val($('#DeliveryDate').val());
        setSelectedForCustomSelectBox('#dialog_LorryType', '#LorryType');
        setSelectedForCustomSelectBox('#dialog_DeliveryAddress', '#DeliveryAddress');
        setSelectedForCustomSelectBox('#dialog_Article', '#Article');
    }

    var confirmDialog = function (yesCallback, noCallback) {
        $('#confirmDelete').removeClass('hidden');
        $('#btnYes').unbind().click(function () {
            $('#confirmDelete').addClass('hidden');
            yesCallback();
        });
        $('#btnNo').unbind().click(function () {
            $('#confirmDelete').addClass('hidden');
            noCallback();
        });
    }

    var showInformationDialog = function (title, message) {
        $('#informationDialog').find('#dialogTitle').html(title);
        $('#informationDialog').find('#dialogContent').html(message);
        $('#informationDialog').removeClass('hidden');
    }

    var showChangeNotAvailableDiglog = function () {
        $('#changeNotAvailable').removeClass('hidden');
    }

    var deleteDeliveryAssurance = function (self, totalSelector, ioNumber, lineNumber) {
        confirmDialog(function () {
            var loader = $(self).parents('.deliveryTableContainer').addClass('disabled').find('.loader-wrapper');
            loader.show();
            $.ajax({
                dataType: "json",
                type: 'post',
                url: '/api/delivery-assurance/delete',
                data: { ioNumber: ioNumber, lineNumber: lineNumber },
                cache: false,
                success: function (data) {
                    if (data.success) {
                        var total = parseInt($(totalSelector).val());
                        if (!isNaN(total) && total >= 1) {
                            total = total - 1;
                            $(totalSelector).val(total);
                        }
                        $(self).parents('tr').remove();
                    } else {
                        showInformationDialog('', data.message);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showInformationDialog('', errorThrown.toString());
                },
                complete: function (jqXHR) {
                    loader.hide().parents('.deliveryTableContainer').removeClass('disabled');
                }
            });
        }, function () {

        });
    }

    var printMultiLines = function (cbxClassName) {
        var ioNumbers = "";
        var lineNumbers = "";
        var rowSelected = 0;

        $('.' + cbxClassName).each(function () {
            if ($(this).is(':checked')) {
                rowSelected++;
                // Get the value. Value contains of string with this format "IOnumber;lineNumber".
                var cbxValue = $(this).val();

                if (cbxValue) {
                    var valSplit = cbxValue.split(';');
                    if (valSplit != null) {

                        ioNumbers += valSplit[0] + "|";
                        lineNumbers += valSplit[1] + "|";
                    }
                }
            }
        });

        if (rowSelected === 0) {
            alert("Du måste först välja rader");
            return;
        }
        // Remove the last pipe line |
        if (ioNumbers !== "")
            ioNumbers = ioNumbers.slice(0, -1);

        if (lineNumbers !== "")
            lineNumbers = lineNumbers.slice(0, -1);

        // Create link to pdfHandler.
        var hrefLink = "/api/pdfhandler/generatemultipdf?a={ioNumber}&l={lineNumber}&resync=true";

        hrefLink = hrefLink.replace("{ioNumber}", ioNumbers); //ex: ioNumbers = "4022316|4023510"
        hrefLink = hrefLink.replace("{lineNumber}", lineNumbers); // ex: lineNumber =  "3|5"

        // Invoke now pdfHandler to create the pdf file.
        window.open(hrefLink, '_blank');
    }

    var getWarehouseList = function (deliveryDateSelector, article) {
        $.ajax({
            dataType: "json",
            type: 'post',
            url: '/api/delivery-assurance/get-warehouse',
            data: { action: $('#Action').val(), customerNumber: $('#dialog_CustomerNumber').val(), itemName: article, deliveryDateStr: $(deliveryDateSelector).val() },
            cache: false,
            success: function (data) {
                if (data.warehouses) {
                    $('.warehouseDropdown').html('');
                    $.each(data.warehouses, function (index, item) {
                        $('.warehouseDropdown')
                            .append('<li data-value="' + item.Key + '"><a href="javascript:void(0)">' + item.Value + '</a></li>');

                    });

                    setDropdown($('.warehouseDropdown').parents('ul.lm__form-dropdown'), 'type-3');

                    $('#warehouseList').find(".showcase .dropdown li a").click(function () {
                        var inputElement = $(this).parents(".showcase").find("input.form-element");
                        //console.log(inputElement.val());
                        var inputSelector = inputElement.attr('id');
                        $("#deliveryAssuranceForm").validate().element('#' + inputSelector);
                    });
                }

                if (data.depaAvtals) {

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                showInformationDialog('', errorThrown.toString());
            },
            complete: function (jqXHR) {
            }
        });
    }

    var validateDeliveryAssurance = function () {
        if ($().validate) $('#deliveryAssuranceForm').validate({
            ignore: [],
            errorElementClass: 'error',
            errorClass: 'error-item',
            errorElement: 'span',
            rules: {
                Quantity: {
                    required: true,
                    max: 39,
                    min: 1
                },
                Torkat: {
                    required: true
                },
                Red: {
                    required: true
                },
                Straforkortat: {
                    required: true
                },
                Slam: {
                    required: true
                },
                HarvestYear: {
                    required: true
                },
                DeliveryType: {
                    required: true
                },
                DeliveryAddress: {
                    required: true
                },
                DeliveryDate: {
                    required: true,
                    date:true
                },
                DepaAvtal: {
                    required: function (element) {
                        return $('input[name="DeliveryType"]:checked').val() === "Depa";
                    }
                },
                Article: {
                    required: true
                },
                Warehouse: {
                    required: function (element) {
                        return enabledWarehouse == "1" && !$('#warehouseList').hasClass('hidden') && !$('#Gardshamtning').is(':checked');
                    }
                },
                Gardshamtning: {
                    required: function (element) {
                        return $('input[name="TermAndCondition"]:checked').length === 0;
                    }
                },
                TermAndCondition: {
                    required: function (element) {
                        return $('input[name="Gardshamtning"]:checked').length === 0;
                    }
                }
            },
            messages: {
                Quantity: {
                    required: "Du måste ange Kvantitet (ton)",
                    max: "Vänligen ange ett värde mellan 1 och 39",
                    min: "Vänligen ange ett värde mellan 1 och 39"
                },
                Torkat: {
                    required: "Du måste ange Varmluftstorkat"
                },
                Red: {
                    required: "Du måste ange RED"
                },
                Straforkortat: {
                    required: "Du måste ange Stråförkortning"
                },
                Slam: {
                    required: "Du måste ange Slammat"
                },
                HarvestYear: {
                    required: "Du måste ange Skördeår"
                },
                DeliveryType: {
                    required: "Du måste ange Överleverans"
                },
                DeliveryAddress: {
                    required: "Du måste ange Leveransadress"
                },
                DeliveryDate: {
                    required: "Du måste ange Leveransdatum",
                    date: "Var vänlig ange ett giltigt datum"
                },
                DepaAvtal: {
                    required: "Inget depåavtal valt. Om vald leveranstyp är Depåavtal behöver ett depåavtal också väljas från listan."
                },
                Article: {
                    required: "Du måste ange Artikel"
                },
                Warehouse: {
                    required: "Du måste ange Mottagningsplats"
                },
                Gardshamtning: {
                    required: "Du måste ange Transportsätt"
                },
                TermAndCondition: {
                    required: ""
                }
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
            },
            errorPlacement: function (error, element) {
                var typeElement = $(element).attr('type');
                if (typeElement === 'radio') {
                    element.parent('div.lm__radio').parent().before(error);
                } else {
                    element.after(error);
                }
            },
            submitHandler: function (form) {
                $('#loader').show();
                $('#loader').parent().addClass('disabled');
                if (existDepaAvtal === 1 && choosenSpontanAvtal === 1) {
                    alert("Det finns depåavtal på vald artikel. Ändra Överleverans till Depåavtal eller bekräfta ditt val av spontanleverans genom att klicka på knappen Nästa en gång till.");
                }

                form.submit();
            }
        });

        if ($().validate) $('#deliveryAssuranceFormStep2').validate({
            ignore: [],
            errorElementClass: 'error',
            errorClass: 'error-item',
            errorElement: 'span',
            rules: {
                noOfNew: {
                    required: function (element) {
                        return $('input[name="createNew"]:checked').val() === "1";
                    },
                    digits: true
                }
            },
            messages: {
                noOfNew: {
                    required: "Du måste ange en siffra",
                    digits: "Ange endast siffror"
                }
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
            }
        });
    }

    var initEvent = function () {
        $('#Gardshamtning').change(function () {
            if ($(this).is(':checked')) {
                $('input[name="TermAndCondition"]:radio').prop('checked', false);
                $('#lorryType').removeClass('hidden');
                if (isInternal != "1") {
                    $('#warehouseList').addClass('hidden');
                }
                $("#deliveryAssuranceForm").validate().element("#Warehouse");
                $("#deliveryAssuranceForm").validate().element("#DeliveryDate");
            }
        });

        $('input[name="TermAndCondition"]:radio').change(function () {
            if ($(this).is(':checked')) {
                $('#Gardshamtning').prop('checked', false);
                $('#lorryType').addClass('hidden');
                if (isInternal != "1") {
                    $('#warehouseList').removeClass('hidden');
                }
                $("#deliveryAssuranceForm").validate().element('#Gardshamtning');
                $("#deliveryAssuranceForm").validate().element("#DeliveryDate");
                $("#deliveryAssuranceForm").validate().element("#Warehouse");
            }
        });

        $('input[name="DeliveryType"]:radio').change(function () {
            var value = $('input[name="DeliveryType"]:checked').val();
            if (value === "Depa") {
                $('#depaAvtal').removeClass('hidden');
                if (choosenSpontanAvtal !== 2) {
                    choosenSpontanAvtal = 0;
                }
            } else if (value === "Spon") {
                $('#depaAvtal').addClass('hidden');
                //Clear selected value for depaAvtal
                if (existDepaAvtal === 1 && choosenSpontanAvtal !== 2) {
                    choosenSpontanAvtal = 1;
                }
            }
        });

        $('input[name="createNew"]:checkbox').change(function () {
            $('#noOfNew').val('');
            if ($(this).is(':checked')) {
                $('#noOfNew').parent().show();
            } else {
                $('#noOfNew').parent().hide();
                $('#createPdf').prop('disabled', false);
            }
        });

        $('#approveTerm').change(function () {
            if ($(this).is(':checked')) {
                $('#btnSubmitDelivery').addClass('reverse-state').removeClass('disabled');
            } else {
                $('#btnSubmitDelivery').addClass('disabled').removeClass('reverse-state');
            }
        });

        $('.cbxApproveDelAss').change(function () {
            if ($('.cbxApproveDelAss').is(':checked')) {
                $('#btnApprove').addClass('reverse-state').removeClass('disabled');
            } else {
                $('#btnApprove').addClass('disabled').removeClass('reverse-state');
            }
        });

        $('.cbxToPrintDeliveried').change(function () {
            if ($('.cbxToPrintDeliveried').is(':checked')) {
                $('#btnToPrintDeliveried').removeClass('disabled');
            } else {
                $('#btnToPrintDeliveried').addClass('disabled');
            }
        });

        $('.cbxToPrintConfirmed').change(function () {
            if ($('.cbxToPrintConfirmed').is(':checked')) {
                $('#btnToPrintConfirmed').removeClass('disabled');
            } else {
                $('#btnToPrintConfirmed').addClass('disabled');
            }
        });

        $('#noOfNew').blur(function () {
            var noOfNew = parseInt($(this).val());
            if (!isNaN(noOfNew) && noOfNew > 0) {
                var cb = $('#createPdf');
                cb.prop('checked', false);
                cb.prop('disabled', true);
            }
        });

        $('#openCalculateFee').click(function () {
            fillCalculateForm();
            $('#calculateFeeDialog').removeClass('hidden');
        });

        $('#calculateFree').click(function () {
            var calculateObject = getCalculateObject();
            $('#calculateFeeDialog').find('.loader-wrapper').show();
            $.ajax({
                url: '/api/deliveryfee/caluculate',
                type: 'POST',
                data: {
                    supplier: calculateObject.supplier,
                    lorryType: calculateObject.lorryType,
                    deliveryAddressId: calculateObject.deliveryAddressId,
                    quantity: calculateObject.quantity,
                    deliveryDate: calculateObject.deliveryDate,
                    itemId: calculateObject.itemId
                },
                success: function (data) {
                    $('#calculateResult').html(data);
                },
                complete: function (jqXHR) {
                    $('#calculateFeeDialog').find('.loader-wrapper').hide();
                }
            });
        });

        $('#clearBtn').click(function() {
            //clear delivery address
            $('#a_deliveryAddress').html('-- Välj leveransadress --');
            $('#DeliveryAddress').val('');
            //clear date
            $('#DeliveryDate').val('');
            //clear article
            $('#a_article').html('-- Välj artikel --');
            $('#Article').val('');
            //clear quantity
            $("#Quantity").val('');
            //clear year
            $('#a_HarvestYear').html('');
            $('#HarvestYear').val('');
            //clear Mottagningsplats
            $('#a_Warehouse').html('-- Välj mottagningsplats --');
            $('#Warehouse').val('');
            $('.warehouseDropdown').html('');
            //clear Leveranssätt
            $('#a_LorryType').html('-- Välj transportsätt --');
            $('#LorryType').val('');

            //clear delivery type
            $('input[name="DeliveryType"]').prop('checked', false);
            //clear Varmluftstorkat
            $("input[name='Torkat']").prop('checked', false);
            //clear RED
            $("input[name='Red']").prop('checked', false);
            //clear Straforkortat
            $("input[name='Straforkortat']").prop('checked', false);
            //clear Slam
            $("input[name='Slam']").prop('checked', false);

            $("input[name='TermAndCondition']").prop('checked', false);
            $('#Gardshamtning').prop('checked', false);

            $("textarea[name='OtherInfo']").val("");

            $("#deliveryAssuranceForm").validate().resetForm();
            //$('#btnSubmitDelivery').addClass('disabled').removeClass('reverse-state');
            $('#depaAvtal').addClass('hidden');
            $('#lorryType').addClass('hidden');
            if (isInternal != "1") {
                $('#warehouseList').addClass('hidden');
            }
        });

        $('#deliveryAssuranceForm').find(".showcase .dropdown li a").click(function () {
            var inputElement = $(this).parents(".showcase").find("input.form-element");
            //console.log(inputElement.val());
            var inputSelector = inputElement.attr('id');
            $("#deliveryAssuranceForm").validate().element('#' + inputSelector);
        });
    }

    var initVar = function () {

        var preDaysDelivery = parseInt($('#preDaysDelivery').val());
        var postDaysDelivery = parseInt($('#postDaysDelivery').val());
        if (isNaN(preDaysDelivery) || isNaN(postDaysDelivery)) return;

        reqDate = GroCommon.dateFromISO($('#reqDate').val());
        startDate = GroCommon.dateFromISO($('#reqDate').val());
        startDate.setHours(0, 0, 0, 0);
        startDate.setDate(reqDate.getDate() + preDaysDelivery);

        endDate = GroCommon.dateFromISO($('#reqDate').val());
        endDate.setHours(23, 59, 59, 999);
        endDate.setDate(reqDate.getDate() + postDaysDelivery);

        harvestPeriodStart = GroCommon.dateFromISO($('#harvestPeriodStart').val());
        harvestPeriodEnd = GroCommon.dateFromISO($('#harvestPeriodEnd').val());

        isInternal = $('#IsInternal').val();
        enabledWarehouse = $('#EnabledWarehouse').val();

        var numberOfDepaAvtal = parseInt($('#numberOfDepaAvtal').val());
        if (!isNaN(preDaysDelivery)) {
            if (numberOfDepaAvtal > 1) {
                $('input[name="DeliveryType"]:radio').each(function (index, item) {
                    var value = $(item).val();
                    if (value === "Depa") {
                        $(item).prop('checked', true);
                    }
                });
                existDepaAvtal = 1;
            } else {
                $('input[name="DeliveryType"]:radio').each(function (index, item) {
                    var value = $(item).val();
                    if (value === "Depa") {
                        $(item).prop('checked', false);
                        $(item).prop('disabled', true);
                        //$(item).parent().addClass('disabled');
                    }
                    if (value === "Spon") {
                        $(item).prop('checked', true);
                    }
                });
                existDepaAvtal = 0;
            }
        }

        $('#deliveryAssuranceForm .dropdown li.selected, #calculateFeeDialog .dropdown li.selected').each(function (index, item) {
            var description = $(item).find('a').html();
            $(item).parent('ul').prev('a').html(description);
        });
    }

    var initDeliveryDate = function () {
        if ($.fn.datepicker) $(".delivery-datepicker").datepicker({
            defaultDate: new Date(),
            onSelect: function (newText) {
                // compare the new value to the previous one
                if ($(this).data('previous') !== newText) {
                    if ($("#deliveryAssuranceForm").validate().element("#DeliveryDate")) {
                        getWarehouseList(this, $("#Article").val());
                    };
                }
            },
            monthNames :["Januari", "Februari", "Mars", "April", "Maj", "Juni", "Juli", "Augusti", "September", "Oktober", "November", "December"],
            dayNamesMin: ["Sö", "Må", "Ti", "On", "To", "Fr", "Lö"],
            firstDay: 1,
        })
        .on('change', function () {
            var $this = $(this);
            var validDate = !/Invalid|NaN/.test(new Date($this.val()).toString());
            var validDateRegex = /^\d\d\d\d-(0?[1-9]|1[0-2])-(0?[1-9]|[12][0-9]|3[01])$/ig.test($this.val());

            $("#deliveryAssuranceForm").validate().element("#DeliveryDate");

            if (!validDateRegex || !validDate) {
                //$this.addClass("error").next().show();
                //$this.next().show();
            } else {
                //$this.removeClass("error").next().hide();
                //$this.next().hide();
                getWarehouseList(this, $("#Article").val());
            }

            });
    }

    var addValidation = function () {
        if (jQuery.validator) {
            jQuery.validator.addMethod("dateHigherThanToday", function (value, element) {
                //If false, the validation fails and the message below is displayed
                var currentDate = new Date();

                var valGardshamtning = $('input[id="Gardshamtning"]:checked').val();
                if (valGardshamtning === '1') {
                    if (!element)
                        return false;

                    var selectedDate = GroCommon.dateFromISO(element.value);  // new Date(element.value); funkar bara med IE9+

                    selectedDate.setHours(0, 0, 0, 0);

                    var currentHours = currentDate.getHours();
                    currentDate.setHours(0, 0, 0, 0);

                    if (selectedDate < currentDate)
                        return false;

                    if ((selectedDate.getFullYear() === currentDate.getFullYear()) &&
                        (selectedDate.getMonth() === currentDate.getMonth()) &&
                        (selectedDate.getDate() === currentDate.getDate()) && (currentHours > 9))
                        return false;

                }
                return true;
            }, "Leveransdatum kan inte vara dagens datum eller tidigare.");

            if (startDate && reqDate) {
                jQuery.validator.addMethod('dateHarvest206', function (value, element) {
                    if (this.optional(element)) {
                        return true;
                    }
                    var enteredDate = GroCommon.dateFromISO(element.value); //new Date(element.value);
                    var tomorrow = new Date().setDate(new Date().getDate() + 1);

                    return ((tomorrow <= enteredDate) && (enteredDate <= reqDate));

                }, $.validator.format("Vänligen ange ett datum mellan {0} och {1}.", startDate ? startDate.toShortDateString() : "", reqDate ? reqDate.toShortDateString() : ""));
            }

            if (harvestPeriodStart && harvestPeriodEnd) {
                jQuery.validator.addMethod('dateHarvestRange', function (value, element) {
                    if (this.optional(element)) {
                        return true;
                    }

                    var enteredDate = GroCommon.dateFromISO(element.value); //new Date(element.value);

                    return ((enteredDate >= harvestPeriodStart) && (enteredDate <= harvestPeriodEnd));

                }, $.validator.format("Vänligen ange ett datum inom skördeperioden, dvs {0}/{1} - {2}/{3}.", harvestPeriodStart.getDate().toString(), (harvestPeriodStart.getMonth() + 1).toString(), harvestPeriodEnd.getDate().toString(), (harvestPeriodEnd.getMonth() + 1).toString()));
            }

            if (startDate && endDate) {
                jQuery.validator.addMethod('daterange', function (value, element) {
                    if (this.optional(element)) {
                        return true;
                    }
                    var enteredDate = GroCommon.dateFromISO(element.value); //new Date(element.value);

                    return ((startDate <= enteredDate) && (enteredDate <= endDate));

                }, $.validator.format("Vänligen ange ett datum mellan {0} och {1}.", startDate ? startDate.toShortDateString() : '', endDate ? endDate.toShortDateString() : ''));
            }
        }
    }

    var init = function () {
        initDeliveryDate();
        initVar();
        initEvent();
        addValidation();
        validateDeliveryAssurance();
    }

    return {
        init: init,
        deleteDeliveryAssurance: deleteDeliveryAssurance,
        showChangeNotAvailableDiglog: showChangeNotAvailableDiglog,
        printMultiLines: printMultiLines,
        getWarehouseList: getWarehouseList
    };
})();
var VerticalTable = VerticalTable || function () {
  var i, element;
  //create headers for the mobile view
  function createHeader(table) {
      var headers = table.querySelectorAll(".responsive-table th"),
      index = 1,
      columns = table.querySelectorAll(".responsive-table td"),
      headerCell,
      headerName,
      headerClass,
      sharedRow,
      responsiveHeader,
      collapseRow;

    if (columns.length > 0) {
      for (i = 0; i < columns.length; i += 1) {
        if (index > headers.length) {
          index = 1;
        }

        //mobile headers are actually td
        element = columns[i];

        if (index === 1) {
          var buttonDiv = document.createElement("div");
          buttonDiv.className = "table-expand-info expand-icon-plus responsive-header";
          element.parentNode.insertBefore(buttonDiv, element);
        }

        headerCell = table.querySelector('.responsive-table th:nth-of-type(' + index + ')');
        headerClass = headerCell.className;
        //console.log(headerClass);
        headerName = headerCell.textContent;
        if (headerClass.indexOf('action-control') >= 0 || headerClass.indexOf('header-desktop-only') >= 0) {
          //console.log(headerCell.textContent);
        } else {
          responsiveHeader = document.createElement("td");
          responsiveHeader.className = "responsive-header " + headerClass;
          responsiveHeader.innerHTML = headerName;
          element.parentNode.insertBefore(responsiveHeader, element);
        }

        index += 1;
      }
    }

    collapseRow = $('.responsive-table .collapsed-row');
    collapseRow.addClass('hidden');
  };

  var hasBeenSet = false;

  //addds mobile class for table when table is larger than screen
  function styleTable() {
    var windowWidth = window.innerWidth,
      responsiveTables = document.getElementsByClassName('responsive-table'),
      collapseRow = $('.responsive-table .collapsed-row');

    if (windowWidth <= 768) {
      if (!hasBeenSet) {
        for (i = 0; i < responsiveTables.length; i += 1) {
          element = responsiveTables[i];
          element.classList.add("mobile");
          collapseRow.addClass('hidden');
          $(".table-expand-info").removeClass('expand-icon-minus');
          $(".table-expand-info").addClass('expand-icon-plus');
        }
        hasBeenSet = true;
      }
    } else {
      for (i = 0; i < responsiveTables.length; i += 1) {
        element = responsiveTables[i];
        element.classList.remove("mobile");
        collapseRow.removeClass('hidden');
        $(".table-expand-info").removeClass('expand-icon-plus');
        $(".table-expand-info").addClass('expand-icon-minus');
      }
      hasBeenSet = false;
    }
  }

  function init() {
      var verticalTables = document.querySelectorAll('table.responsive-table');
      if (verticalTables.length > 0) {
          for (var count = 0; count < verticalTables.length; count++) {
              createHeader(verticalTables[count]);
              //console.log(count);
          }
      }

    styleTable();
    $(window).on('resize', function () {
      styleTable();
    });
  }
  return {
    init: init
  }
} ();

$(function () {
  VerticalTable.init();
});

$(document).ready(function (e) {
  $(".table-expand-info").click(function () {
    var currentRow = $($(this).parent());
    var collapseRow = currentRow.find('.collapsed-row');
    if ($(this).hasClass("expand-icon-plus")) {
      $(this).removeClass("expand-icon-plus");
      $(this).addClass("expand-icon-minus");
      collapseRow.removeClass('hidden');
    }
    else {
      $(this).removeClass("expand-icon-minus");
      $(this).addClass("expand-icon-plus");
      collapseRow.addClass('hidden');
    }
  });
  $(".table-expand-info").next().click(function () {
    var currentRow = $($(this).parent());
    var that = currentRow.find('.table-expand-info')[0];
    var collapseRow = currentRow.find('.collapsed-row');
    if ($(that).hasClass("expand-icon-plus")) {
      $(that).removeClass("expand-icon-plus");
      $(that).addClass("expand-icon-minus");
      collapseRow.removeClass('hidden');
    }
    else {
      $(that).removeClass("expand-icon-minus");
      $(that).addClass("expand-icon-plus");
      collapseRow.addClass('hidden');
    }
  });
});
jQuery(function () {
    PurchaseAgreement.init();
    PriceAlertPage.init();
    DryingAgreement.init();
    GrainPricePage.init();
    PoolAndDepaAgreement.init();
});

var PurchaseAgreement = PurchaseAgreement || (function () {
    var agreementElements = [];
    var disabledClass = "disabled";
    var formSelector = "#agreementForm";
    
    var addValidation = function () {
        if (jQuery.validator) {
            jQuery.validator.addMethod("exact_date", function (value, element) {
                var result = true;
                var dates = value.split('-');

                if (dates.length == 3) {
                    //var d = new Date(dates[0] + "-" + dates[1] + "-" + dates[2] + " 00:00:00");
                    var d = new Date(value);
                    if (d.getFullYear() == dates[0] && d.getMonth() == (parseInt(dates[1]) - 1) && d.getDate() == dates[2]) {
                        result = true;
                    } else {
                        result = false;
                    }
                }

                return this.optional(element) || result;

            }, "Var vänlig ange ett giltigt datum");

            jQuery.validator.addMethod("lowerPriceMustLessUpperPrice", function (value, element) {
                //If false, the validation fails and the message below is displayed
                var upperPrice = parseInt($('#UpperPrice').val());
                var lowerPrice = parseInt($('#LowerPrice').val());
                return this.optional(element) || (!isNaN(upperPrice) && !isNaN(lowerPrice) && upperPrice >= lowerPrice);

            }, "Undre pris måste vara lägre än Övre pris");

            jQuery.validator.addMethod("atLeastOnePriceMustBeInput", function (value, element) {
                //If false, the validation fails and the message below is displayed
                var upperPrice = parseInt($('#UpperPrice').val());
                var lowerPrice = parseInt($('#LowerPrice').val());
                if (isNaN(lowerPrice)) lowerPrice = 0;
                return this.optional(element) || (!isNaN(upperPrice) && !isNaN(lowerPrice) && (upperPrice !== 0 || lowerPrice !== 0));

            }, "Något pris kanske ska anges?");

            jQuery.validator.addMethod("optinalLowerPriceMustBeGreatMinValue", function (value, element) {
                //If false, the validation fails and the message below is displayed
                var minValue = parseInt($('#PriceLow').val());
                var lowerPrice = parseInt($('#LowerPrice').val());
                if ($('#AgreementType').val() === "PrissakringDepaavtal") {
                    return this.optional(element) || (!isNaN(minValue) && !isNaN(lowerPrice) && !(lowerPrice >= 0 && lowerPrice < minValue));
                }
                return this.optional(element) || (!isNaN(minValue) && !isNaN(lowerPrice) && !(lowerPrice > 0 && lowerPrice < minValue));

            }, "Undre pris understiger. Pris anges i kr/ton");

            jQuery.validator.addMethod("agreementDateMin", function (value, element) {
                //If false, the validation fails and the message below is displayed
                var minDate = new Date();
                var enteredDate = GroCommon.dateFromISO(element.value); //new Date(element.value);
                enteredDate.setHours(enteredDate.getHours() + 23);

                return this.optional(element) || (enteredDate >= minDate);

            }, "Du kan inte ange datum som är lägre än dagens datum");

            jQuery.validator.addMethod("agreementDateMax", function (value, element) {
                //If false, the validation fails and the message below is displayed
                var minDate = new Date();
                var maxDate = new Date();
                maxDate.setDate(new Date().getDate() + 60);
                var enteredDate = GroCommon.dateFromISO(element.value); //new Date(element.value);
                enteredDate.setHours(enteredDate.getHours() + 23);

                return this.optional(element) || (enteredDate <= maxDate);

            }, "Uppdraget får ej vara giltigt i mer än 60 dagar");
        }
    }

    var validatePriceHedge = function () {
        var lowPrice = GroCommon.parseInt($('#PriceLow').val());
        var minQuantity = GroCommon.parseInt($('#CommitQuantityMin').val());

        if ($().validate) $(formSelector).validate({
            ignore: [],
            errorElementClass: 'error',
            errorClass: 'error-item',
            errorElement: 'span',
            onkeyup: false,
            rules: {
                UpperPrice: {
                    required: true,
                    digits: true,
                    atLeastOnePriceMustBeInput: true,
                    min: lowPrice
                },
                LowerPrice: {
                    required: function (element) {
                        return $('#AgreementType').val() === "PrissakringDepaavtal";
                    },
                    digits: true,
                    lowerPriceMustLessUpperPrice: true,
                    optinalLowerPriceMustBeGreatMinValue: true
                },
                CommitQuantity: {
                    required: true,
                    digits: true,
                    min: minQuantity
                },
                DeliveryMode: {
                    required: true
                },
                PriceWatchEndDate: {
                    date: true,
                    exact_date: true,
                    agreementDateMin: true,
                    agreementDateMax: true,
                    //required:true
                },
                TargetAction: {
                    required: true
                }
            },
            messages: {
                UpperPrice: {
                    required: "Du måste ange en siffra",
                    digits: "Ange endast siffror",
                    atLeastOnePriceMustBeInput: "Något pris kanske ska anges?",
                    min: "Övre pris understiger " + lowPrice + " kr. Pris anges i kr/ton"
                },
                LowerPrice: {
                    required: "Du måste ange en siffra",
                    digits: "Ange endast siffror",
                    lowerPriceMustLessUpperPrice: "Undre pris måste vara lägre än Övre pris",
                    optinalLowerPriceMustBeGreatMinValue: "Undre pris understiger " + lowPrice + " kr. Pris anges i kr/ton."
                },
                CommitQuantity: {
                    required: "Kvantitet ton måste anges",
                    digits: "Kvantitet ton anges numeriskt",
                    min: "Du kan inte ange kvantitet som är mindre än " + minQuantity
                },
                DeliveryMode: {
                    required: "Leveranssätt måste väljas"
                },
                PriceWatchEndDate: {
                    date: "Var vänlig ange ett giltigt datum"
                },
                TargetAction: {
                    required: "Du måste ange Resultat av prisbevakning"
                }
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
            },
            errorPlacement: function (error, element) {
                var typeElement = $(element).attr('type');
                if (typeElement === 'radio') {
                    element.parent('div.lm__radio').parent().before(error);
                } else {
                    element.after(error);
                }
            },
            submitHandler: function (form) {
                if ($('#approveAgreementTerm').is(':checked')) {
                    form.submit();
                }
            }
        });
    }

    var toggleDisableNextButton = function () {
        var commitQuantity = parseInt($('#CommitQuantity').val());
        var upperPrice = parseInt($('#UpperPrice').val());
        var lowerPrice = parseInt($('#LowerPrice').val());

        var isValid = $('#AgreementAvtal').val() != '' &&
            ($('input[name="DeliveryMode"]:checked').length > 0 || $('input[name="DeliveryMode"]:radio').length === 0) &&
            ($('input[name="TargetAction"]:checked').length > 0 || $('input[name="TargetAction"]:radio').length === 0) &&
            $('#AgreementPeriod').val() != '' &&
            $('#PriceWatchEndDate').val() != '' &&
            ((!isNaN(commitQuantity) && commitQuantity >= 12) || $('#CommitQuantity').length === 0) &&
            !isNaN(upperPrice) && upperPrice >= 500 &&
            !isNaN(lowerPrice) && lowerPrice <= upperPrice && lowerPrice >= 500;

        if (isValid)
            $('#move-to-step2').addClass('reverse-state').removeClass('disabled');
        else
            $('#move-to-step2').addClass('disabled').removeClass('reverse-state');
    }

    var validateForDropdown = function (self) {
        var inputElement = $(self).parents(".showcase").find("input.form-element");
        var inputSelector = inputElement.attr('id');
        $(formSelector).validate().element('#' + inputSelector);
        toggleDisableNextButton();
    }
    
    var initPriceWatchEndDate = function () {
        if ($.fn.datepicker) $("#PriceWatchEndDate").datepicker({
            defaultDate: new Date(),
            onSelect: function (newText) {
                // compare the new value to the previous one
                if ($(this).data('previous') !== newText) {
                    $(formSelector).validate().element("#PriceWatchEndDate");
                    toggleDisableNextButton();
                }
            },
            monthNames: ["Januari", "Februari", "Mars", "April", "Maj", "Juni", "Juli", "Augusti", "September", "Oktober", "November", "December"],
            dayNamesMin: ["Sö", "Må", "Ti", "On", "To", "Fr", "Lö"],
            firstDay: 1,
            showOtherMonths: true,
            selectOtherMonths: true,
            dateFormat: 'yy-mm-dd',
            beforeShow: function () {
                setTimeout(function () {
                    $('.ui-datepicker').css('z-index', 5);
                }, 0);
            }
        })
        .on('change', function () {
            $(formSelector).validate().element("#PriceWatchEndDate");
            toggleDisableNextButton();
        });
    }

    var getProtectAgreement = function (agreementId, productItemId, priceAreaId, grainType, productItemName, callback) {
        if (!agreementId) {
            return;
        }
        $('#agreement-loader').show();
        $("#AgreementId").val(agreementId);
        $("#product-item-name").html(productItemName);
        $("#grain-type").html(grainType);

        var apiUrl = "/api/agreement/get-protect-agreement";
        $.ajax({
            dataType: "json",
            type: 'get',
            url: apiUrl,
            data: {
                productItemId: productItemId,
                priceAreaId: priceAreaId,
                grainType: grainType
            },
            cache: false,
            success: function (data) {
                $("#agreement-detail").show();
                if (data) {
                    $("#price-periods").html($.parseHTML(data.periodsView));
                    setDropdown($("#price-periods"), "type-2");
                    $("#price-periods .showcase a.period-item").each(function (idx, item) {
                        $(item).click(function () {
                            GroCommon.disableOrEnableElement('#agreementForm .agreement-element', $(item));
                            toggleDisableNextButton();
                        });
                    });
                    callback();
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                
            },
            complete: function (jqXHR) {
                $('#agreement-loader').hide();
            }
        });
    }

    var getDisplayValueInDropdown = function ($dropdown, selectedValue) {
        var result;
        $dropdown.find('li').each(function (index, item) {
            var matchValue = $(item).attr('data-value');
            if (matchValue === selectedValue) {
                result = $(item).find('a').html();
                return false;
            }
        });

        return result;
    }

    var getHarvestYear = function (periodInfo) {
        var startIndex = periodInfo.indexOf(';');
        var datePeriod = periodInfo.substring(startIndex + 1, startIndex + 9);
        var year = parseInt(datePeriod.substring(0, 4));
        var date = parseInt(datePeriod.substring(4));
        if (isNaN(year) || isNaN(date)) return "";

        return date < 701 ? year - 1 : year;
    }

    var getGrainTypesForSpotAgreement = function (priceAreaId, productItemId) {
        $('#agreement-loader').show();
        $('#product-item-name').html(getDisplayValueInDropdown($('.productDropdown .dropdown'), productItemId));
        $.ajax({
            dataType: "json",
            type: 'get',
            url: '/api/agreement/get-grain-type-spot-agreement',
            data: { priceAreaId: priceAreaId, productItemId: productItemId },
            cache: false,
            success: function (data) {
                $('#agreementForm .grainTypeDropdown .dropdown').html('');
                $('#agreementForm .grainTypeDropdown > .showcase > a').html('Välj sort');
                $('#agreementForm #GrainType').val('');
                $('#grain-type').html('');
                if (data.length > 0) {
                    $.each(data, function (index, item) {
                        $('.grainTypeDropdown .dropdown').append('<li data-value="' + item.GrainName + '"><a href="javascript:void(0)">' + item.GrainName + '</a></li>');
                    });

                    setDropdown($('.grainTypeDropdown'), 'type-3');

                    $('#agreementForm .grainTypeDropdown').find(".showcase .dropdown li a").click(function () {
                        validateForDropdown(this);
                        GroCommon.disableOrEnableElement('#agreementForm .agreement-element', $(this));
                        $('#grain-type').html(getDisplayValueInDropdown($('.grainTypeDropdown .dropdown'), $(this).parent().attr('data-value')));
                        if ($('#AgreementPeriod').val() != '') {
                            $('.agreement-detail').removeClass('hidden');
                        }
                    });

                    $('#agreementForm .grainTypeDropdown').parent().parent().removeClass('disabled');
                }
                $('#agreementForm .periodDropdown').parent().parent().addClass('disabled');
                $('.agreement-detail').addClass('hidden');
                toggleDisableNextButton();
            },
            error: function (jqXHR, textStatus, errorThrown) {
            },
            complete: function (jqXHR) {
                $('#agreement-loader').hide();
            }
        });
    }

    
    var initEvent = function () {
        $('#UpperPrice').blur(function () {
            $(formSelector).validate().element("#LowerPrice");
            toggleDisableNextButton();
        });

        $('#approveAgreementTerm').change(function () {
            if ($(this).is(':checked')) {
                $('#agreementSaveBtn').addClass('reverse-state').removeClass('disabled');
            } else {
                $('#agreementSaveBtn').addClass('disabled').removeClass('reverse-state');
            }
        });

        $("#agreementForm .showcase .dropdown a.agreement-item").each(function (idx, item) {
            $(item).click(function() {
                var $item = $(item);
                var agreementId = $item.attr("data-agreementid");
                var productItemId = $item.attr("data-productItemId");
                var priceAreaId = $item.attr("data-priceAreaId");
                var grainType = $item.attr("data-grainType");
                var productItemName = $item.attr("data-productItemName");
                getProtectAgreement(agreementId, productItemId, priceAreaId, grainType, productItemName, function () {
                    GroCommon.disableOrEnableElement('#agreementForm .agreement-element', $(item));
                    toggleDisableNextButton();
                });
                showOverview();
            });
        });
        function showOverview() {
            $("#area-sort-overview").removeClass("hidden");
        }

        $("#agreementForm .agreement-element input").each(function (idx, item) {
            $(item).focusout(function () {
                GroCommon.disableOrEnableElement('#agreementForm .agreement-element', $(item));
                toggleDisableNextButton();
            });
        });

        $('#agreementForm input[name="TargetAction"]:radio').change(function () {
            GroCommon.disableOrEnableElement('#agreementForm .agreement-element', $(this));
            toggleDisableNextButton();
        });

        $('#agreementForm input[name="DeliveryMode"]:radio').change(function () {
            GroCommon.disableOrEnableElement('#agreementForm .agreement-element', $(this));
            toggleDisableNextButton();
        });

        $('#move-to-step2').click(function () {
            if ($().validate) {
                var isValid = $(formSelector).valid();
                if (isValid) {
                    $('#span_Avtal').html(getDisplayValueInDropdown($('.avtalDropdown .dropdown'), $('#AgreementAvtal').val()));
                    $('#span_Groda').html($('#product-item-name').html());
                    $('#span_Sort').html($('#grain-type').html());
                    $('#span_Leveransperiod').html(getDisplayValueInDropdown($('.periodDropdown .dropdown'), $('#AgreementPeriod').val()) + " " + getHarvestYear($('#AgreementPeriod').val()));
                    $('#span_UpperPrice').html($('#UpperPrice').val());
                    $('#span_UnderPrice').html($('#LowerPrice').val());

                    $('#span_Quantity').html($('#CommitQuantity').val());
                    $('#span_TransportType').html($('input[name="DeliveryMode"]:checked').prop('id'));
                    $('#span_Prissakra').html($('input[name="TargetAction"]:checked').prop('id'));
                    $('#span_AgreementDate').html($('#PriceWatchEndDate').val());

                    $('#pStep1').addClass('hidden');
                    $('#pStep2').removeClass('hidden');
                }
            }
        });

        $('#move-to-step1').click(function () {
            $('#pStep2').addClass('hidden');
            $('#pStep1').removeClass('hidden');
        });

        $(formSelector).find(".showcase .dropdown li a").click(function () {
            validateForDropdown(this);
            //disableOrEnableElement($(this));
            GroCommon.disableOrEnableElement('#agreementForm .agreement-element', $(this));
        });

        $('#agreementForm .periodDropdown .dropdown a').click(function () {
            $('.agreement-detail').removeClass('hidden');
            $('#lable_Period').html(getDisplayValueInDropdown($('.periodDropdown .dropdown'), $(this).parent().attr('data-value')));
        });

        $("a#agrement-info").click(function() {
            $("#agreement-introduce").removeClass("hidden");
        });
    }

    var init = function () {
        // init total-favourite-items
        $('#CommitQuantity, #UpperPrice, #LowerPrice').val('');
        addValidation();
        validatePriceHedge();
        initEvent();
        initPriceWatchEndDate();
        agreementElements = $('.agreement-element');
    }

    return {
        init: init,
        getProtectAgreement: getProtectAgreement,
        getGrainTypesForSpotAgreement: getGrainTypesForSpotAgreement
    };
})();

var PriceAlertPage = PriceAlertPage || (function () {

    var deletePriceAlert = function (priceWatch, id) {
        var parameters = "{'id':'" + id + "'}";
        $.ajax({
            type: 'POST',
            url: '/api/price-alert/DeletePriceWatch',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: parameters,
            success: function (response) {
                if (response.isRemoved) {
                    $(priceWatch).remove();
                }
                else {
                    alert("Error");
                }
            },
            error: function (response) { }
        });
    }

    var deletePriceAlertEvent = function () {
        $(".priceAlertDelete").on("click", function (e) {
            var confirmDelete = window.confirm("Är du säker på att du vill ta bort detta prisuppdraget ?");
            if (confirmDelete) {
                var id = $(this).attr("data-value");
                var itemPriceWatch = $(this).parents("tr");
                deletePriceAlert(itemPriceWatch, id);
            }
            e.preventDefault();
        });
    }

    var initEvent = function () {
        deletePriceAlertEvent();
    }

    var init = function () {
        initEvent();
    }

    return {
        init: init
    };
})();

var GrainPricePage = GrainPricePage || (function () {

    var getPricePeriodbyArea = function (id) {
        var tableResult = $("#price-period-result");
        var parameters = "{'priceAreaId':'" + id + "'}";
        $("#loader").show();
        $.ajax({
            type: 'POST',
            url: '/api/price-grain/GetPricePeriodbyArea',
            contentType: "application/json; charset=utf-8",
            data: parameters,

            success: function (response) {
                if (response.resultPricePeriods == undefined) {
                    tableResult.html(response);
                }
                else {
                    tableResult.empty();
                    alert("Error");
                }
                $("#loader").hide();
            },
            error: function () {
                tableResult.empty();
                alert("Error");
                $("#loader").hide();
            }
        });
    }

    var getPricePeriodbyAreaEvent = function () {
        var dropdownPriceAreas = $('#price-area-list .dropdown li a');
        dropdownPriceAreas.on("click", function (e) {
            var priceAreaId = $(this).parent().attr('data-value');
            getPricePeriodbyArea(priceAreaId);
            e.preventDefault();
        });
    }

    var initEvent = function () {
        getPricePeriodbyAreaEvent();
    }

    var init = function () {
        initEvent();
    }

    return {
        init: init
    };
})();

var DryingAgreement = DryingAgreement || (function () {

    var initEvent = function () {
        $('#approveDryingAgreementTerm').change(function () {
            if ($(this).is(':checked')) {
                $('#approveDryingAgreementTermInStep2').prop('checked', true);
                $('#dryingBtnMoveToStep2, #dryingBtnMoveToStep3').addClass('reverse-state').removeClass('disabled');
            } else {
                $('#approveDryingAgreementTermInStep2').prop('checked', false);
                $('#dryingBtnMoveToStep2, #dryingBtnMoveToStep3').addClass('disabled').removeClass('reverse-state');
            }
        });

        $('#dryingBtnMoveToStep2').click(function () {
            $('#dryingStep1').addClass('hidden');
            $('#dryingStep2').removeClass('hidden');
        });

        $('#dryingBtnBackToStep1').click(function () {
            $('#dryingStep2').addClass('hidden');
            $('#dryingStep1').removeClass('hidden');
        });

        if ($('#generateDryingPdf').length > 0) {
            $('#generateDryingPdf')[0].click();
        }
    }

    var init = function () {
        initEvent();
    }

    return {
        init: init
    };
})();

var PoolAndDepaAgreement = PoolAndDepaAgreement || (function () {

    var validateForDropdown = function (self) {
        var inputElement = $(self).parents(".showcase").find("input.form-element");
        var inputSelector = inputElement.attr('id');
        $("#newPurchasingAgreementForm").validate().element('#' + inputSelector);
        toggleDisableNextButton();
    }

    var enableDeliveryMode = function () {
        $('.div-DeliveryMode').removeClass('disabled');
    }

    var getDisplayValueInDropdown = function ($dropdown, selectedValue) {
        var result;
        $dropdown.find('li').each(function (index, item) {
            var matchValue = $(item).attr('data-value');
            if (matchValue === selectedValue) {
                result = $(item).find('a').html();
                return false;
            }
        });

        return result;
    }

    var getHarvestYear = function (periodInfo) {
        var startIndex = periodInfo.indexOf(';');
        var datePeriod = periodInfo.substring(startIndex + 1, startIndex + 9);
        var year = parseInt(datePeriod.substring(0, 4));
        var date = parseInt(datePeriod.substring(4));
        if (isNaN(year) || isNaN(date)) return "";

        return date < 701 ? year - 1 : year;
    }

    var validateForm = function () {
        if ($().validate) $('#newPurchasingAgreementForm').validate({
            ignore: [],
            errorElementClass: 'error',
            errorClass: 'error-item',
            errorElement: 'span',
            onkeyup: false,
            rules: {
                CommitQuantity: {
                    required: true,
                    digits: true,
                    min: 12
                },
                DeliveryMode: {
                    required: true
                },
                ProductItemId: {
                    required: true
                },
                GrainType: {
                    required: true
                },
                AgreementPeriod: {
                    required: true
                }
            },
            messages: {
                CommitQuantity: {
                    required: "Kvantitet ton måste anges",
                    digits: "Kvantitet ton anges numeriskt",
                    min: "Du kan inte ange kvantitet som är mindre än 12"
                },
                DeliveryMode: {
                    required: "Transportsätt måste väljas"
                },
                ProductItemId: {
                    required: "Gröda måste väljas"
                },
                GrainType: {
                    required: "Sort måste väljas"
                },
                AgreementPeriod: {
                    required: "Leveranssätt måste väljas"
                }
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass(this.settings.errorElementClass).removeClass(errorClass);
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass(this.settings.errorElementClass).removeClass(errorClass);
            },
            errorPlacement: function (error, element) {
                var typeElement = $(element).attr('type');
                if (typeElement === 'radio') {
                    element.parent('div.lm__radio').parent().before(error);
                } else {
                    element.after(error);
                }
            },
            submitHandler: function (form) {
                if ($('#approvePoolAgreementTerm').is(':checked')) {
                    form.submit();
                }
            }
            
        });
    }

    var toggleDisableNextButton = function () {
        var commitQuantity = parseInt($('#CommitQuantity').val());
        var isValid = $('#ProductItemId').val() != '' &&
            $('#GrainType').val() != '' &&
            $('input[name="DeliveryMode"]:checked').length > 0 &&
            $('#AgreementPeriod').val() != '' &&
            !isNaN(commitQuantity) && commitQuantity >= 12;
        if (isValid)
            $('#btnMoveToStep2').addClass('reverse-state').removeClass('disabled');
        else
            $('#btnMoveToStep2').addClass('disabled').removeClass('reverse-state');
    }

    var getGrainTypes = function (priceAreaId, productItemId) {
        $('#agreement-loader').show();
        $.ajax({
            dataType: "json",
            type: 'get',
            url: '/api/agreement/get-grain-type',
            data: { priceAreaId: priceAreaId, productItemId: productItemId },
            cache: false,
            success: function (data) {
                $('.grainTypeDropdown .dropdown').html('');
                $('.grainTypeDropdown > .showcase > a').html('Välj sort');
                $('#GrainType').val('');
                if (data.length > 0) {
                    $.each(data, function (index, item) {
                        $('.grainTypeDropdown .dropdown').append('<li data-value="' + item.GrainName + '"><a href="javascript:void(0)" onclick="PoolAndDepaAgreement.enableDeliveryMode()">' + item.GrainName + '</a></li>');
                    });

                    setDropdown($('.grainTypeDropdown'), 'type-3');

                    $('.grainTypeDropdown').find(".showcase .dropdown li a").click(function () {
                        validateForDropdown(this);
                    });

                    $('.grainTypeDropdown').parent().parent().removeClass('disabled');
                }
                toggleDisableNextButton();
            },
            error: function (jqXHR, textStatus, errorThrown) {
            },
            complete: function (jqXHR) {
                $('#agreement-loader').hide();
            }
        });
    }

    var initEvent = function () {
        $('#newPurchasingAgreementForm input[name="DeliveryMode"]:radio').change(function () {
            GroCommon.disableOrEnableElement('#newPurchasingAgreementForm .agreement-element', $(this));
            toggleDisableNextButton();
        });

        $('#newPurchasingAgreementForm .periodDropdown .dropdown a').click(function () {
            GroCommon.disableOrEnableElement('#newPurchasingAgreementForm .agreement-element', $(this));
        });

        $('#approvePoolAgreementTerm').change(function () {
            if ($(this).is(':checked')) {
                $('#agreementSaveBtn').addClass('reverse-state').removeClass('disabled');
            } else {
                $('#agreementSaveBtn').addClass('disabled').removeClass('reverse-state');
            }
        });

        $('#btnMoveToStep2').click(function () {
            if ($().validate) {
                var isValid = $("#newPurchasingAgreementForm").valid();
                if (isValid) {
                    $('#span_Groda').html(getDisplayValueInDropdown($('.productDropdown .dropdown'), $('#ProductItemId').val()));
                    $('#span_Sort').html(getDisplayValueInDropdown($('.grainTypeDropdown .dropdown'), $('#GrainType').val()));
                    $('#span_Period').html(getDisplayValueInDropdown($('.periodDropdown .dropdown'), $('#AgreementPeriod').val()) + " " + getHarvestYear($('#AgreementPeriod').val()));

                    $('#span_Quantity').html($('#CommitQuantity').val());
                    $('#span_TransportType').html($('input[name="DeliveryMode"]:checked').prop('id'));

                    $('#pStep1').addClass('hidden');
                    $('#pStep2').removeClass('hidden');
                }
            }
        });

        $('#btnBackToStep1').click(function () {
            $('#pStep2').addClass('hidden');
            $('#pStep1').removeClass('hidden');
        });

        $('#newPurchasingAgreementForm').find(".showcase .dropdown li a").click(function () {
            validateForDropdown(this);
        });

        $('#newPurchasingAgreementForm #CommitQuantity').blur(function () {
            toggleDisableNextButton();
        });
    }

    var init = function () {
        initEvent();
        validateForm();
    }

    return {
        init: init,
        getGrainTypes: getGrainTypes,
        enableDeliveryMode: enableDeliveryMode
    };
})();

$(document).ready(function () {
    var myTimeBooking = new TimeBookingPage();
    var myListingBooking = new ListingBookingPage();
    myTimeBooking.init();
    myListingBooking.init();
    $(document).trigger("enhance.tablesaw");
});
var blockUI = function () {
    $("#loader").show();
}

var unBlockUI = function () {
    $("#loader").hide();
}
var fillContentToModal = function (dialogId, header, content, openDialog) {
    var dialogArea = $(dialogId);
    dialogArea.find(".lm__wide-modal__title").html(header);
    dialogArea.find("p").append(content);
    dialogArea.find(".btn-ok").on("click", function () {
        dialogArea.addClass("hidden");
    });
    if (openDialog === undefined) {
        dialogArea.removeClass("hidden");
    }
}
var clearContentToModal = function (dialogId) {
    var dialogArea = $(dialogId);
    dialogArea.find(".lm__wide-modal__title").html("");
    dialogArea.find("p").html("");
    dialogArea.find(".btn-ok").unbind("click");
}
var fillContentToDeleteDialog = function (header, content) {
    var dialogArea = $("#deleteDialog");
    dialogArea.find(".success-header-title").html(header);
    dialogArea.find("p").append(content);
    dialogArea.find(".btn-cancel").on("click", function () {
        dialogArea.hide();
    });
    dialogArea.find(".btn-ok").on("click", function () {
        dialogArea.hide();
    });
    dialogArea.show();
}
var clearContentToDeleteDialog = function () {
    var dialogArea = $("#deleteDialog");
    dialogArea.find(".lm__wide-modal__title").html("");
    dialogArea.find("p").html("");
    dialogArea.find(".btn-ok").unbind("click");
    dialogArea.find(".btn-cancel").unbind("click");
}

var TimeBookingPage = function () {
    var clearResultTable = function () {
        $("#resultTable").html('');
    }

    var extractForDropdown = function (elId, extractFrom) {
        var arr = new Array();
        for (var i = 0; i < extractFrom.length; i++) {

            arr[arr.length] = '<li data-value="' + extractFrom[i].Value + '"><a href="#">' + extractFrom[i].Display + '<\/a><\/li>';
            if (extractFrom[i].IsSelected == true) {
                $('#' + elId + '> a').html(extractFrom[i].Display);
                if (elId == 'resourceDrp') {
                    $('#' + elId).attr('regNoMandatory', extractFrom[i].RegNoMandatory);
                }
                $('#' + elId).attr('data-value', extractFrom[i].Value);
            }
        }
        return arr.join('');
    }
    //Search customer
    var searchCustomer = function () {
        clearResultTable();
        //setCustomerInfo(null);
        $('#agrementDriedUnDried').hide();
        var searchString = $('#seachFieldTxt').val();
        var resourceValue = $('#resourceDrp').attr('data-value');
        if (jQuery.trim(searchString) != '') {
            var searchType = $('#search-option').attr('data-value');
            var loadOrUnlodValue = $("input[name='loadOrUnlodRad']:checked").val();
            var customerSearcType = $("input[name='companySelected']:checked").val();
            blockUI();
            var parameters = "{'resourceGroupId':'" + resourceValue + "', 'searchString':'" + searchString + "', 'searchType':'" + searchType + "', 'loadOrUnlodValue':'" + loadOrUnlodValue + "', 'customerSearcType':'" + customerSearcType + "'}";
            $.ajax({
                type: 'POST',
                url: '/api/time-booking/CustomerSearch',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: parameters,

                success: function (response) {
                    unBlockUI();
                    searchCustomerSuccess(response.customerResult);
                    //showLinkToDeliveryAssurance(response.customerResult);
                },
                error: function (response) {
                    unBlockUI();
                    //searchCustomerError(response); 
                }
            });
        }

        var searchCustomerSuccess = function (response) {
            var dialogHtml;
            if (response.Status == 500) {
                clearContentToModal("#dialogArea");
                dialogHtml = response.ErrorMessage + '<br/><br/>';
                fillContentToModal("#dialogArea", "Fel uppstod", dialogHtml);
                return;
            }
            var customers = (typeof response.Customers) == 'string' ? eval('(' + response.Customers + ')') : response.Customers;
            setCustomerInfo(customers);
            if ($('#search-option').attr("data-value") === '9' && response.Ios.length > 0) {
                clearContentToModal("#dialogArea");
                dialogHtml = '<p>Inköpsordrar kopplade till kundnummer: ' + response.Customers[0].CustomerNo + '</p>';
                dialogHtml += '<p>Klicka på någon av nedanstående inköpsordrar för att gå direkt till den</p><br/>';
                dialogHtml += '<table style="width: 580px; display: block; overflow-y: scroll; height: 300px;">' +
                    '<thead><tr>' +
                    '<th>IONummer</th><th>Artikel</th><th>Kvantitet</th><th>Anläggning</th></tr></thead>' +
                    '<tbody>';
                $.each(response.Ios, function (i, d) {
                    dialogHtml += '<tr><td><a href="?ion=' +
                        d.IONumber +
                        '">' +
                        d.IONumber +
                        '</a></td><td>' +
                        d.ItemName +
                        '</td><td>' +
                        d.Quantity +
                        '</td><td>' +
                        d.WarehouseName +
                        '</td></tr>';
                });
                dialogHtml += '</tbody>' + '</table>';
                fillContentToModal("#dialogArea", "Välj inköpsorder nedan", dialogHtml);
            }

            if (response.BookingOrder !== null) {
                if (response.BookingOrder.Quantity != undefined && response.BookingOrder.Quantity !== null) {
                    $('#quantityTxt').val(response.BookingOrder.Quantity / 1000);

                }

                if (response.BookingOrder.DeliveryDate !== null) {
                    $('#selectedDate').val(new Date(parseInt(response.BookingOrder.DeliveryDate.substr(6))).toShortDateString());
                    if ($('#selectedDate').val().length < 10) {
                        $('#selectedDate').val(new Date().toShortDateString());
                    }
                }
                if ($('#search-option').attr("data-value") === '5') {
                    if (response.BookingOrder.Status > 40) {
                        unblockUI();
                    } else if (response.BookingOrder.Status == 40) {
                        unblockUI();
                    }
                    else if (response.BookingOrder.DeliveryAssuranceConfirmed == false) {
                        clearContentToModal("#dialogArea");
                        fillContentToModal("#dialogArea", "Redan bokad", 'Obs! Bokning ej tillåten på denna inköpsorder! Leveransförsäkran saknas');
                    }
                    getIOResource(response.BookingOrder.Warehouse);
                    searchItems('', $('#resourceDrp').attr('data-value'));
                }
                else if ($('#search-option').attr("data-value") === '7' || $('#search-option').attr("data-value") === '8' || $('#search-option').attr("data-value") === '6') {
                    getIOResource(response.BookingOrder.Warehouse);
                    searchItems('', $('#resourceDrp').attr('data-value'));
                }

                if (response.BookingOrder.ItemNumber !== null && response.BookingOrder.ItemNumber != '') {
                    var dried = 'torkad';
                    var $articlesDrp = $('#articlesDrp');
                    $("#agrementDriedUnDried").val(response.BookingOrder.Torkat ? 'Torkad' : 'Otorkad');
                    var driedUnDried = $("#agrementDriedUnDried").val();

                    if (!response.BookingOrder.Torkat) {
                        dried = 'otorkad';
                    }

                    var searchString = new String(response.BookingOrder.ItemNumber + '-' + dried + '-' + response.BookingOrder.Sort);

                    searchString = searchString.toLowerCase();
                    $('#articlesDrp > ul li').each(function () {
                        if ($(this).attr("data-value").toLowerCase().indexOf(searchString) != -1) {
                            $articlesDrp.attr("data-value", $(this).attr("data-value"));
                            $articlesDrp.find(">a").html($(this).find(">a").html());
                        }
                    });
                }

                $("div").data("Linenumber", response.BookingOrder.Linenumber);
            }
            if (response.RegNo !== null) {
                $('#IDReg').val(response.RegNo);
            }
            if (validateRequiredFieldsForSearch()) {
                $("#searchAvailbleSlotsBtn").parent().removeClass("disabled-btn");
                searchAvailableSlots();
                if ($('#search-option').attr('data-value') == 5) {
                    $('#IDLevnr_StartValue').val($('#seachFieldTxt').val());
                } else {
                    $('#IDLevnr_StartValue').val('');
                }
            }
            else {
                $('#searchAvailbleSlotsBtn').parent().addClass("disabled-btn");
                $('#IDLevnr_StartValue').val('');
            }
        };

        var getIOResource = function (warehouse) {
            var resourceListLoadSuccess = function (response, warehouseId) {
                var resource = $("#resourceDrp");
                clearDropDown('resourceDrp', 'Välj anläggning');
                clearDropDown('articlesDrp', 'Välj artikel');
                //clearDropDown('veichleTypeDrp', '- Välj fordon -');
                var resources = (typeof response) == 'string' ? eval('(' + response + ')') : response;
                $(resource).find('> ul > li').append(extractForDropdown('resourceDrp', resources));
                setDropdown($('#resourceDrp').parent(), 'type3');
                for (var i = 0; i < resources.length; i++) {
                    if ((warehouseId !== undefined && response[i].M3Id === warehouseId) ||
                        (warehouseId !== undefined && response[i].Value === warehouseId)) {
                        $(resource).attr("data-value", resources[i].Value);
                        $(resource).find(">a").html(resources[i].Display);
                        break;
                    }
                }
                validateRequiredFieldsEvent();
                searchItemEvent();
            };
            $.ajax({
                type: 'POST',
                url: '/api/time-booking/LoadResourceGroupsOnIO',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{'warehouseId':'" + warehouse + "'}",
                async: false,
                success: function (response) {
                    resourceListLoadSuccess(response.Resource, warehouse);
                },
                error: function (response) {
                    clearDropDown('resourceDrp', 'Välj anläggning');
                    clearDropDown('articlesDrp', 'Välj artikel');
                    clearDropDown('veichleTypeDrp', 'Välj fordon');
                    alert('Error');
                }
            });


        };

        var setCustomerInfo = function (customers) {
            if (customers != null) {

                var arr = new Array();
                var noOfCustomers = customers.length;


                if (noOfCustomers == 1) {
                    $('#customerNo').val(customers[0].CustomerNo);
                    $('#customerName').val(customers[0].Name);
                    $('#IDEpost_StartValue').val(customers[0].Email);
                    $('#IDMobil_StartValue').val(customers[0].MobileNo);

                    $('#IDEpost').val(customers[0].Email);
                    $('#IDMobil').val(customers[0].MobileNo);
                    displayWarningAboutDemoCustomer(customers[0].Name);

                } else {
                    clearContentToModal("#dialogArea");
                    fillContentToModal("#dialogArea", "Information saknas", 'Hittade ingen kund, avtal, transportorder eller sändningsorder med nr ' + $('#seachFieldTxt').val() + '.');
                }

            }
        };

        var displayWarningAboutDemoCustomer = function (customerName) {
            customerName = customerName.toLowerCase();
            if ((customerName.indexOf('testkund') >= 0) || (customerName.indexOf('demokund') >= 0)) {
                clearContentToModal("#dialogArea");
                fillContentToModal("#dialogArea", "Demokund", 'Obs! Du försöker boka på en test eller demokund vilket orsakar problem i vissa fall. Gör därför alltid bokningen i rätt kundnummer!');
            }
        }
    };

    //Update reservation
    var clearForm = function () {
        var reminder = $("#IDReminder");
        reminder.attr("data-value", "0");
        reminder.find(">a").html("Ingen påminnelse");

        $("#IDOvrigt").val('');
        $("#IDLevnr").val($("#IDLevnr_StartValue").val());
        $("#IDReg").val('');
        $('#IDEpost').val($('#IDEpost_StartValue').val());
        $('#IDMobil').val($('#IDMobil_StartValue').val());
        $('#saveAction').val('New');
        $('#updateBtn').hide();
        $('#saveBtn').show();

        $('#myDisplayMessage').html('');
        $('#dispalyMessageHeader').html('&nbsp;');


        //$('#newCustomerHeading').attr('disabled', 'disabled');
        $('#newCustomerNo').attr('disabled', 'disabled');
    }
    var resetForm = function () {
        $('#agrementDriedUnDried').val('Torkad');
        $('#agrementDriedUnDried').hide();

        //$('#newCustomerHeading').attr('disabled', 'disabled');
        $('#newCustomerNo').attr('disabled', 'disabled');

        $('#searchAvailbleSlotsBtn').parent().addClass("disabled-btn");
        $('#showReservationStopps').hide();

        /*  Clears form from editing values.*/
        $('.fieldToClearWithClearForm').val('');
        $('#IDEpost').val('');
        $('#IDEpost_StartValue').val('');

        $('#IDMobil').val('');
        $('#IDMobil_StartValue').val('');
        $('#saveAction').val('New');

        $('#updateBtn').hide();
        $('#saveBtn').show();


        $('#myDisplayMessage').html('');
        $('#dispalyMessageHeader').html('&nbsp;');



        $('#resourceDrp').attr('data-value', '');
        $('#resourceDrp > a').html('Välj anläggning');

        $('#articlesDrp').attr('data-value', '');
        $('#articlesDrp > a').html('Välj artikel');
        $('#loadOrUnlodRad').val('1');

        $('#veichleTypeDrp').attr('data-value', '');
        $('#veichleTypeDrp > a').html('Välj fordon');
        $('#quantityTxt').val('');
    }
    var copyValuesToEnableEdit = function (editObj) {

        $("div").data("dateRegistered", editObj.dateRegistered);
        $("#dispalyMessageHeader").html('Ändra bokning genom att ändra uppgifter nedan och klicka Uppdatera bokning.');
        $('#myDisplayMessage').html(editObj.displayMessage);

        $('#IDLevnr').val(editObj.idLevnr);
        $('#IDReg').val(editObj.idReg);
        $('#IDOvrigt').val(editObj.idOvrigt);
        var reminder = $("#IDReminder");
        if (editObj.idReminder === "0" || editObj.idReminder === "") {
            reminder.attr("data-value", "");
            reminder.find(">a").html("Ingen påminnelse");
        } else {
            reminder.attr("data-value", editObj.idReminder);
            var subReminder = reminder.find('ul > li[data-value="' + editObj.idReminder + '"]');
            var valueSubReminder = subReminder.find("a").html();
            subReminder.addClass("selected");
            reminder.find(">a").html(valueSubReminder);
        }
        $('#IDEpost').val(editObj.idEpost);
        $('#IDMobil').val(editObj.idMobil);
        $('#saveBtn').hide();
        $('#updateBtn').show();

        $('#saveAction').val('Update');
        $('#ReservationID').val(editObj.reservationId);

        $('#SpeditorNo_ForUpdate').val(editObj.speditorNo);

        $('#CustomerNo_ForUpdate').val(editObj.customerNo);
        $('#CustomerName_ForUpdate').val(editObj.customerName);

        $('#newCustomerNo').val(editObj.customerNo);
        $('#newCustomerName').val(editObj.customerName);

        $('#TransportOrderNo_ForUpdate').val(editObj.transportOrderNo);
        $('#ContractNo_ForUpdate').val(editObj.contractNo);
        $('#ReminderEmail_ForUpdate').val(editObj.reminderEmail);
        $('#ReminderSMS_ForUpdate').val(editObj.reminderSms);
        $('#Owner').val(editObj.owner);

        //$('#newCustomerHeading').removeAttr('disabled');
        $('#newCustomerNo').removeAttr('disabled');
        $('#newCustomerNo').val(editObj.customerNo);
        $('input:checkbox').attr('checked', false);
        $("#SaveTable").show();
    }
    var updateReservation = function () {
        var updateReservationSuccess = function (response) {
            var arr = new Array();

            arr[arr.length] = '<p>';
            for (var i = 0; i < response.length; i++) {

                if (response[i].Status == 200) {
                    arr[arr.length] = response[i].Message;
                } else {
                    arr[arr.length] = response[i].ErrorMessage;
                }
                arr[arr.length] = '<br \/>';
            }

            arr[arr.length] = '<\/p>';

            clearContentToModal("#dialogArea");
            clearForm();
            //unblockUI();
            fillContentToModal("#dialogArea", "Bokningen uppdaterad", arr.join(''));
            searchAvailableSlots();
        }

        var updateReservationError = function (response) {
            clearContentToModal("#dialogArea");
            fillContentToModal("#dialogArea", "Uppdateringsfel", "Fel uppstod när bokningen skulle uppdateras.");
            searchAvailableSlots();
        }

        var errors = validateFieldsForReservation(true);

        if (errors.length > 0) {
            clearContentToModal("#dialogArea");
            var arr = new Array();
            arr[arr.length] = '<p>';
            arr[arr.length] = '<ul>';
            arr[arr.length] = errors.join('');
            arr[arr.length] = '<\/ul>';
            arr[arr.length] = '<\/p>';
            fillContentToModal("#dialogArea", "Uppgifter saknas", arr.join(''));
        } else {

            var oldCustomerNo = $('#CustomerNo_ForUpdate').val();
            var oldCustomerName = $('#CustomerName_ForUpdate').val();

            var newCustomerNo = $('#newCustomerNo').val();
            var newCustomerName = $('#newCustomerName').val();
            var loadUnLoadVale = $('#loadOrUnlodRad').val();
            var mobileNo = $('#IDMobil').val();
            var leveransforsakransnummer = $('#IDLevnr').val();
            var note = $('#IDOvrigt').val();
            var reminderInMinutesBefore = $('#IDReminder').attr("data-value");
            var licensePlateNo = $('#IDReg').val();
            var email = $('#IDEpost').val();
            var owner = $('#Owner').val();
            var reservationId = $('#ReservationID').val();
            var speditor = $('#SpeditorNo_ForUpdate').val();

            if (validateSaveTable(email, mobileNo, licensePlateNo, leveransforsakransnummer)) {
                var innerDto = new Object();
                innerDto['Note'] = note;
                innerDto['ReminderInMinutesBefore'] = reminderInMinutesBefore;
                innerDto['EmailAddress'] = email;
                innerDto['NewCustomerName'] = newCustomerName;
                innerDto['NewCustomerNo'] = newCustomerNo;
                innerDto['OldCustomerName'] = oldCustomerName;
                innerDto['OldCustomerNo'] = oldCustomerNo;

                innerDto['Leveransforsakransnummer'] = leveransforsakransnummer;
                innerDto['LicensePlateNo'] = licensePlateNo;
                innerDto['MobilePhone'] = mobileNo;
                innerDto['Note'] = note;
                innerDto['ReminderInMinutesBefore'] = reminderInMinutesBefore;
                innerDto['EmailAddress'] = email;
                innerDto['SpeditorNo'] = speditor;

                innerDto['ReservationId'] = reservationId;
                innerDto['Owner'] = owner;
                innerDto['LineNumber'] = $("div").data("Linenumber");
                innerDto['DateRegistered'] = $("div").data("dateRegistered");

                var containerDto = new Object();
                containerDto.reservationToUpdate = innerDto;

                $.ajax({
                    type: 'POST',
                    url: '/api/time-booking/UpdateReservation',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(containerDto),

                    success: function (response) { updateReservationSuccess(response.updateReservations); },
                    error: function (response) { updateReservationError(response.updateReservations); }
                });
            }
        }
    }

    //Delete reservation
    var deleteReservation = function (reservationId, time, owner, customerNo, dateRegistered) {
        var deletReservationSuccess = function (response) {
            var arr = new Array();
            arr[arr.length] = '<p>';
            for (var i = 0; i < response.length; i++) {
                if (response[i].Status == 500) {

                    arr[arr.length] = '<span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>';
                }
                arr[arr.length] = response[i].Message + '<br \/>';
            }
            arr[arr.length] = '<\/p>';

            clearContentToModal("#dialogArea");
            fillContentToModal("#dialogArea", "Borttagning", arr.join(''));
            $("#dialogArea").find(".btn-ok").on("click", function () {
                searchAvailableSlots();
            });
        }

        var deletReservationError = function (response) {
            clearContentToModal("#dialogArea");
            fillContentToModal("#dialogArea", "Raderingsfel", "Fel uppstod när bokningen skulle raderas.");
            searchAvailableSlots();
        }

        var deletReservationProcess = function (reservationId, owner, customerNo, dateRegistered) {
            clearForm();
            var o = new Object();
            o.reservationId = reservationId;
            o.owner = owner;
            o.customerNo = customerNo;
            o.dateRegistered = dateRegistered;
            $.ajax({
                type: 'POST',
                url: '/api/time-booking/DeleteReservation',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(o),

                success: function (response) { deletReservationSuccess(response.deleteReservations); },
                error: function (response) { deletReservationError(response.deleteReservations); }
            });
        }

        var arr = new Array();
        arr[arr.length] = '<p>';
        arr[arr.length] = 'Vill du verkligen radera bokningen<br \/> kl: ' + time + '?';
        arr[arr.length] = '<\/p>';
        clearContentToDeleteDialog();
        fillContentToDeleteDialog("Ta bort bokning?", arr.join(''));
        $("#deleteDialog").find(".btn-ok").on("click", function () {
            deletReservationProcess(reservationId, owner, customerNo, dateRegistered);
        });
    }

    //Search items
    var searchItems = function (currentArticleItem, resource) {
        var searchItemComplete = function (response) {
            var vechicles = (typeof response.Vehicles) == 'string' ? eval('(' + response.Vehicles + ')') : response.Vehicles;
            var reservationStops = (typeof response.ReservationStops) == 'string' ? eval('(' + response.ReservationStops + ')') : response.ReservationStops;
            var articles = (typeof response.Items) == 'string' ? eval('(' + response.Items + ')') : response.Items;
            clearDropDown('articlesDrp', 'Välj artikel');
            $('#articlesDrp > ul').append(extractForDropdown('articlesDrp', articles));
            $('#veichleTypeDrp > ul').append(extractForDropdown('veichleTypeDrp', vechicles));
            setDropdown($('#articlesDrp').parent(), 'type3');
            setDropdown($('#veichleTypeDrp').parent(), 'type3');
            if (vechicles.length > 0) {
                $("#veichleTypeDrp > a").html(vechicles[0].Display);
                $("#veichleTypeDrp").attr("data-value", vechicles[0].Value);
            }
            validateRequiredFieldsEvent();
            buildReservationStops(reservationStops);
        }

        var buildReservationStops = function (reservationStops) {
            var i = 0;
            var arr = new Array();
            for (i = 0; i < reservationStops.length; i++) {
                arr[arr.length] = '<li>' + reservationStops[i].ResourceName + '<\/li>';
                arr[arr.length] = '<li>' + reservationStops[i].FromDate + ' ' + reservationStops[i].FromTime + ' - ';
                if (reservationStops[i].ToDate.length > 0) {
                    arr[arr.length] = reservationStops[i].ToDate + ' ' + reservationStops[i].ToTime + '<\/li>';
                } else {
                    arr[arr.length] = ' tillsvidare<\/li>';
                }
                arr[arr.length] = '<li style="font-style: italic">' + reservationStops[i].Message + '<br \/><\/li>';
            }
            $("#showReservationStopps").find("> ul").html(arr.join(''));
            if (i > 0) {
                $('#showReservationStopps').show();
            } else {
                $('#showReservationStopps').hide();
            }
        }

        var resourceValue = resource;
        var selectedDateValue = $('#selectedDate').val();

        if (!currentArticleItem) {
            currentArticleItem = $('#articlesDrp').attr("data-value");
        }

        var showOnlyUnloadingItems = ($("input:radio[name='loadOrUnlodRad']:checked").val() == '2');
        clearDropDown('articlesDrp', 'Välj artikel');
        clearDropDown('veichleTypeDrp', 'Välj fordon');
        $('#showReservationStopps').hide();
        if (resourceValue != '0' && resourceValue != '' && selectedDateValue != '') {
            blockUI();
            var parameters = "{'resourceGroupId':'" + resourceValue + "', 'selectedDate':'" + selectedDateValue + "', 'currentArticleItem':'" + currentArticleItem + "', 'showOnlyUnloadingItems':'" + showOnlyUnloadingItems + "'}";
            $.ajax({
                type: 'POST',
                url: '/api/time-booking/loadItemsOnresourceGroup',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: parameters,
                async: false,
                success: function (response) {
                    searchItemComplete(response);
                    unBlockUI();
                },
                error: function () {
                    unBlockUI();
                    alert('Error');
                }
            });
        }
    };
    var clearDropDown = function (elId, defaultText) {

        $('#' + elId + '> ul > li').remove();
        if ($.trim(defaultText) != '') {
            $('#' + elId).attr('data-value', '');
            $('#' + elId + '> a').html(defaultText);
        }

    };
    var validateRequiredFieldsForSearch = function () {
        var aRequiredFieldHasNoValue = false;
        $('.searchSlotRequired').each(function () {
            if ($(this).is('input')) {
                if ($.trim($(this).val()).length == 0) {
                    aRequiredFieldHasNoValue = true;
                    if ($(this).is('#quantityTxt')) {
                        $("#Quantity-error").hide();
                        $(this).removeClass("error");
                    }
                }
                else if ($(this).is('#quantityTxt')) {
                    if (!$.isNumeric($.trim($(this).val()))) {
                        aRequiredFieldHasNoValue = true;
                        $("#Quantity-error").show();
                        $(this).addClass("error");
                    } else {
                        $("#Quantity-error").hide();
                        $(this).removeClass("error");
                    }
                }
            }
            if ($(this).is('li')) {
                if ($(this).attr('data-value') == 0 || $(this).attr('data-value') == '') {
                    aRequiredFieldHasNoValue = true;
                }
            }
        });
        return !aRequiredFieldHasNoValue;
    }

    //Searchavailble slots
    var searchAvailableSlots = function () {
        var showHideTimebookingTable = function (isShow) {
            if (isShow) {
                $("#resultTable").show();
                $("#resultTable-box").addClass("has-separator");
            } else {
                $("#resultTable").hide();
                $("#resultTable-box").removeClass("has-separator");
            }
            $("#SaveTable").hide();
            $("#saveBtn").hide();
        }
        var searchAvailableSlotsSuccess = function (response) {
            var hookupEventsForTimeCheckboxes = function () {
                $(".bookingRows").change(function () {
                    var anyChecked = false;
                    $(".bookingRows").each(function () {
                        if ($(this).prop("checked")) {
                            anyChecked = true;
                        }

                    });
                    if (anyChecked) {
                        $("#SaveTable").show();
                        $("#saveBtn").removeClass("disabled-btn");
                        clearForm();
                        clearValidateSaveTable();

                    } else {
                        $("#SaveTable").hide();
                        $("#saveBtn").hide();
                        $("#saveBtn").addClass("disabled-btn");
                    }
                });
            }

            clearResultTable();
            $("#resultTable").append(response);
            if (response !== "") {
                $(".tb-deleteBtn").unbind("click").bind("click", function (e) {
                    var row = $(this).parents(".tb-row");
                    var reservationId = row.find(".tb-reservationId").val();
                    var fromDateTime = row.find(".tb-fromDateTime").val();
                    var owner = row.find(".tb-owner").val();
                    var customerNo = row.find(".tb-customerNo").val();
                    var dateRegistered = row.find(".tb-dateRegistered").val();
                    deleteReservation(reservationId, fromDateTime, owner, customerNo, dateRegistered);
                    e.preventDefault();
                });

                $(".tb-editBtn").unbind("click").bind("click", function (e) {
                    var row = $(this).parents(".tb-row");
                    var editObj = new Object();
                    editObj.idLevnr = row.find(".tb-idLevnr").val();
                    editObj.idReg = row.find(".tb-idReg").val();
                    editObj.idOvrigt = row.find(".tb-idOvrigt").val();
                    editObj.idReminder = row.find(".tb-idReminder").val();
                    editObj.idEpost = row.find(".tb-idEpost").val();
                    editObj.idMobil = row.find(".tb-idMobil").val();
                    editObj.reservationId = row.find(".tb-reservationId").val();
                    editObj.speditorNo = row.find(".tb-speditorNo").val();
                    editObj.customerNo = row.find(".tb-customerNo").val();
                    editObj.customerName = row.find(".tb-customerName").val();
                    editObj.transportOrderNo = row.find(".tb-transportOrderNo").val();
                    editObj.contractNo = row.find(".tb-contractNo").val();
                    editObj.reminderEmail = row.find(".tb-reminderEmail").val();
                    editObj.reminderSms = row.find(".tb-reminderSms").val();
                    editObj.fromDateTime = row.find(".tb-fromDateTime").val();
                    editObj.itemInfo = row.find(".tb-item").html();
                    editObj.quantity = row.find(".tb-quantity").html();
                    editObj.unit = row.find(".tb-unit").html();
                    editObj.owner = row.find(".tb-owner").val();
                    editObj.resourceName = row.find(".tb-resourceName").val();
                    editObj.dateRegistered = row.find(".tb-dateRegistered").val();
                    editObj.displayMessage = "Tid: " + editObj.fromDateTime + ", Kund: " + editObj.customerNo + ' ' + editObj.customerName + ', Bokningsnr: ' + editObj.reservationId +
                        ', Artikel: ' + editObj.itemInfo + ' ' + editObj.quantity + ' ' + editObj.unit + '<br \/>Resurs: ' + editObj.resourceName;
                    copyValuesToEnableEdit(editObj);
                    e.preventDefault();
                });
                $(document).trigger("enhance.tablesaw");
                showHideTimebookingTable(true);
            }
            else {
                $(".tb-deleteBtn").unbind("click");
                $(".tb-editBtn").unbind("click");
                showHideTimebookingTable(false);
            }
            hookupEventsForTimeCheckboxes();
        }
        clearResultTable();
        if (validateRequiredFieldsForSearch()) {
            blockUI();
            var resourceValue = $('#resourceDrp').attr('data-value');
            var selectedDateValue = $('#selectedDate').val();
            var agrementDriedUnDriedValue = $('#agrementDriedUnDried').val();

            var articlesValue = $('#articlesDrp').attr('data-value');
            var quantityTxtValue = $('#quantityTxt').val();
            var loadOrUnlodValue = $("input[name='loadOrUnlodRad']:checked").val();
            var veichleTypeValue = $('#veichleTypeDrp').attr('data-value');

            var searchTypeValue = $('#search-option').attr('data-value');
            var customerNo = $('#customerNo').val();

            if (resourceValue != '' && selectedDateValue != '' && quantityTxtValue != '' && (articlesValue != '' || argementValue != '')) {
                var parameters = "{'resourceGroupId':'" + resourceValue + "', 'selectedDate':'" + selectedDateValue + "', 'article':'" + articlesValue + "', 'qty':'" + quantityTxtValue + "', 'loadunload':'" + loadOrUnlodValue + "', 'veichleType':'" + veichleTypeValue + "', 'driedUnDried':'" + agrementDriedUnDriedValue + "', 'customerNo':'" + customerNo + "','searchType':'" + searchTypeValue + "'}";
                $.ajax({
                    type: 'POST',
                    url: '/api/time-booking/SearchAvailbleSlots',
                    contentType: "application/json; charset=utf-8",
                    data: parameters,

                    success: function (response) {
                        searchAvailableSlotsSuccess(response);
                        unBlockUI();
                    },
                    error: function () {
                        unBlockUI();
                        showHideTimebookingTable(false);
                        alert("Error");
                    }
                });
            }
        }
    };
    var validateFieldsForReservation = function (isUpdate) {
        var arr = new Array();

        if (jQuery.trim($('#quantityTxt').val()) == '') {
            arr[arr.length] = '<li>Kvantitet saknar värde.<\/li>';
        }

        var quantityValue = parseInt(jQuery.trim($('#quantityTxt').val()), 10);
        if (isNaN(quantityValue) || quantityValue <= 0) {
            arr[arr.length] = '<li>Kvantitet måste ha ett värde större än 0.<\/li>';
        }


        if (jQuery.trim($('#customerNo').val()) == '') {
            arr[arr.length] = '<li>Ingen gilltigt kund har valts.<\/li>';
        }

        /* Kontrollera om resursgruppen kräver regnr.*/
        if ($('#IDReg').hasClass('updateRequiered')) {

            if (jQuery.trim($('#IDReg').val()) == '') {
                arr[arr.length] = '<li>Registreringsnummer saknas.<\/li>';
            }
        }

        if (jQuery.trim($('#veichleTypeDrp').attr('data-value')) == '') {
            arr[arr.length] = '<li>Fordonstyp saknas.<\/li>';
        }


        if (jQuery.trim($('#selectedDate').val()) == '') {
            arr[arr.length] = '<li>Datum saknas.<\/li>';
        }


        if (jQuery.trim($('#articlesDrp').attr('data-value')) == '') {
            arr[arr.length] = '<li>Artikel saknas.<\/li>';
        }


        if (jQuery.trim($('#resourceDrp').attr('data-value')) == '') {
            arr[arr.length] = '<li>Resurs saknas.<\/li>';
        }

        if (!isUpdate) {
            var resourceValue = $('#resourceDrp').attr('data-value');
            var selectedDateValue = $('#selectedDate').val();
            var agrementDriedUnDriedValue = $('#agrementDriedUnDried').val();
            var articlesValue = $('#articlesDrp').attr('data-value');
            var quantityTxtValue = $('#quantityTxt').val();
            var loadOrUnlodValue = $("input[name='loadOrUnlodRad']:checked").val();
            var veichleTypeValue = $('#veichleTypeDrp').attr('data-value');
            var searchTypeValue = $('#search-option').attr('data-value');
            var customerNo = $('#customerNo').val();
            var ioNumber = $('#seachFieldTxt').val();

            if (resourceValue != '' && selectedDateValue != '' && quantityTxtValue != '' && (articlesValue != '' || argementValue != '')) {

                //blockUI();
                var parameters = "{'resourceGroupId':'" + resourceValue + "', 'selectedDate':'" + selectedDateValue + "', 'article':'" + articlesValue + "', 'qty':'" + quantityTxtValue + "', 'loadunload':'" + loadOrUnlodValue + "', 'veichleType':'" + veichleTypeValue + "', 'driedUnDried':'" + agrementDriedUnDriedValue + "', 'customerNo':'" + customerNo + "','searchType':'" + searchTypeValue + "', 'iONumber':'" + ioNumber + "'}";
                $.ajax({
                    type: 'POST',
                    url: '/api/time-booking/ExistBooking',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: parameters,
                    async: false,
                    success: function (response) {
                        //unblockUI();
                        if (response.IsExistBooking == true) {
                            arr[arr.length] = "<li>Det finns redan en bokning på inköpsordern. Radera befintlig bokning innan du kan skapa en ny!<\/li>";
                        }
                    },
                    error: function () {
                        //unblockUI();
                        alert('ExistBooking');
                    }
                });
            }
        }
        return arr;
    }

    //Make reservation
    var makeReservation = function () {
        
        var makereservationContinue = function () {
            var reservations = new Array();
            var i = 0;
            var qty = $("#quantityTxt").val();
            var customerNo = $("#customerNo").val();
            var customerName = $("#customerName").val();


            var performUnload = ($("input:radio[name='loadOrUnlodRad']:checked").val() === "2");

            var mobileNo = $('#IDMobil').val();
            var leveransforsakransnummer = $('#IDLevnr').val();
            var note = $('#IDOvrigt').val();
            var reminderInMinutesBefore = $('#IDReminder').attr("data-value");
            var licensePlateNo = $('#IDReg').val() != null ? $('#IDReg').val() : "";
            var dried = $('#agrementDriedUnDried').val();
            var email = $('#IDEpost').val();
            var vehicleAssortmentId = $('#veichleTypeDrp').attr('data-value');
            var searchType = $('#search-option').attr('data-value');
            var searchValue = $('#seachFieldTxt').val();
            var selectedDate = $('#selectedDate').val();
            var item = $('#articlesDrp').attr('data-value');
            var resourceId = $('#resourceDrp').attr('data-value');
            var agrement = '';
            if (validateSaveTable(email, mobileNo, licensePlateNo, leveransforsakransnummer)) {
                $('.bookingRows').each(function () {
                    if ($(this).is(':checked')) {
                        i++;

                        var o = new Object();

                        o['FromTime'] = $(this).attr('fromtime');

                        o['ResourceId'] = $(this).attr('resursid');
                        if (performUnload == true) {
                            o['Loading'] = 'false';
                            o['Unloading'] = 'true';
                        } else {
                            o['Loading'] = 'true';
                            o['Unloading'] = 'false';
                        }
                        reservations.push(o);

                    }

                });

                if (reservations.length > 0) {
                    var innerDTO = new Object();
                    innerDTO['Reservations'] = reservations;
                    innerDTO['Agrement'] = agrement;
                    innerDTO['SearchValue'] = searchValue;
                    innerDTO['Qty'] = qty;
                    innerDTO['VehicleAssortmentID'] = vehicleAssortmentId;
                    innerDTO['ResourceId'] = resourceId;
                    innerDTO['Note'] = note;

                    innerDTO['ReminderInMinutesBefore'] = reminderInMinutesBefore;
                    innerDTO['EmailAddress'] = email;

                    innerDTO['CustomerName'] = customerName;
                    innerDTO['CustomerNo'] = customerNo;
                    innerDTO['Dried'] = dried;
                    innerDTO['SearchType'] = searchType;
                    innerDTO['SelectedDate'] = selectedDate;
                    innerDTO['Item'] = item;
                    innerDTO['Leveransforsakransnummer'] = leveransforsakransnummer;
                    innerDTO['LicensePlateNo'] = licensePlateNo;
                    innerDTO['MobilePhone'] = mobileNo;

                    var Linenumber = $("div").data("Linenumber");
                    if (Linenumber == null) {
                        Linenumber = 0;
                    }

                    innerDTO['LineNumber'] = Linenumber;

                    var containerDTO = new Object();
                    containerDTO.reservationToMake = innerDTO;

                    $.ajax({
                        type: 'POST',
                        url: '/api/time-booking/MakeReservation',
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(containerDTO),

                        success: function (response) {
                            makeReservationSuccess(response.makeReservations);

                        },
                        error: function (response) { makeResrvationError(response); }
                    });
                } else {
                    clearContentToModal("#dialogArea");
                    var arr = new Array();
                    arr[arr.length] = '<p>Inga tider valda<\/p>';
                    fillContentToModal("#dialogArea", "Tider saknas", arr.join(''));
                }
            }

        }
        var makeReservationSuccess = function (response) {
            var arr = new Array();
            var msgTitle = '';
            var successCnt = 0;
            var errorCnt = 0;
            arr[arr.length] = '<h4>Bokningar:<\/h4>';
            arr[arr.length] = '<p>';
            for (var i = 0; i < response.length; i++) {
                if (response[i].Status == 200) {
                    if (i > 0) {
                        arr[arr.length] = '<br \/><br \/>';
                    }
                    arr[arr.length] = response[i].StartDate;
                    arr[arr.length] = ' kl: ';
                    arr[arr.length] = response[i].StartTime;
                    arr[arr.length] = '<br \/>';
                    arr[arr.length] = response[i].CustomerNo;
                    arr[arr.length] = ' ';
                    arr[arr.length] = response[i].CustomerName;
                    successCnt++;
                } else {
                    errorCnt++;

                    arr[arr.length] = response[i].ErrorMessage;
                }
            }
            arr[arr.length] = "<br \/>";
            arr[arr.length] = "<\/p>";
            if (errorCnt > 0 && successCnt === 0) {
                msgTitle = "Fel uppstod vid bokningen";
            } else if (errorCnt > 0 && successCnt > 0) {
                msgTitle = "Alla bokningar gick inte igenom";
            } else if (errorCnt == 0 && successCnt > 0) {
                msgTitle = "Bokningar sparade";
            }

            clearContentToModal("#dialogArea");
            fillContentToModal("#dialogArea", msgTitle, arr.join(""));
            $("#saveBtn").addClass("disabled-btn");
            clearForm();
            searchAvailableSlots();
        }

        var makeResrvationError = function (response) {

            //clearForm();
            clearContentToModal("#dialogArea");
            fillContentToModal("#dialogArea", "Bokningsfel", "Fel uppstod när boking skulle registreras.");
        }

        var errors = validateFieldsForReservation(false);
        if (errors.length > 0) {
            clearContentToModal("#dialogArea");
            var arr = new Array();
            arr[arr.length] = '<p>';
            arr[arr.length] = '<ul>';
            arr[arr.length] = errors.join('');
            arr[arr.length] = '<\/ul>';
            arr[arr.length] = '<\/p>';
            fillContentToModal("#dialogArea", "Uppgifter saknas", arr.join(''));
            return false;
        } else {
            makereservationContinue();
        }
    }

    var searchAvalibleSlotAction = function () {
        clearForm();
        $("#resultTable").hide();
        $("#resultTable-box").removeClass("has-separator");
        $("#SaveTable").hide();
        $("#saveBtn").hide();
        searchAvailableSlots();
    }
    //Event
    var validateButtonSearchEvent = function () {
        $("#seachFieldTxt").unbind('keyup').keyup(function () {
            if ($(this).val() !== "") {
                $("#searcFieldhBtn").removeClass("disabled-btn");
            } else {
                $("#searcFieldhBtn").addClass("disabled-btn");
            }
        });
        $("#seachFieldTxt").unbind('change').change(function () {
            if ($(this).val() !== "") {
                $("#searcFieldhBtn").removeClass("disabled-btn");
            } else {
                $("#searcFieldhBtn").addClass("disabled-btn");
            }
        });
    }

    var makeReservationEvent = function () {
        $("#saveBtn").click(function () { makeReservation(); });
    }

    var updateReservationEvent = function () {
        $("#updateBtn").click(function () { updateReservation(); });
    }

    var validateRequiredFieldsEvent = function () {
        $("#form-time-booking").unbind('change').change(function () {
            if (validateRequiredFieldsForSearch()) {
                $("#searchAvailbleSlotsBtn").parent().removeClass("disabled-btn");
            }
            else {
                $('#searchAvailbleSlotsBtn').parent().addClass("disabled-btn");
            }
        });
        $("#quantityTxt").unbind('keyup').keyup(function () {
            if (validateRequiredFieldsForSearch()) {
                $("#searchAvailbleSlotsBtn").parent().removeClass("disabled-btn");
            }
            else {
                $('#searchAvailbleSlotsBtn').parent().addClass("disabled-btn");
            }
        });
        setDropdown($('#resourceDrp').parent(), 'type3');
        $(".searchSlotRequired .dropdown li a").on('click', function () {
            if (validateRequiredFieldsForSearch()) {
                $('#searchAvailbleSlotsBtn').parent().removeClass("disabled-btn");
            }
            else {
                $('#searchAvailbleSlotsBtn').parent().addClass("disabled-btn");
            }
        });
        if (validateRequiredFieldsForSearch()) {
            $('#searchAvailbleSlotsBtn').parent().removeClass("disabled-btn");
        }
        else {
            $('#searchAvailbleSlotsBtn').parent().addClass("disabled-btn");
        }
    }

    var searchItemEvent = function () {
        var dropdownResource = $('#resourceDrp .dropdown li a');
        dropdownResource.on('click', function () {
            var resourceValue = $(this).parent().attr('data-value');
            searchItems('', resourceValue);
            var regNoMandatory = $(this).parent().attr('regNoMandatory');
            if (regNoMandatory != null) {
                if (regNoMandatory.toLowerCase() == 'true') {
                    $('#IDReg').addClass('updateRequiered');
                } else {
                    $('#IDReg').removeClass('updateRequiered');
                }
            } else {
                $('#IDReg').removeClass('updateRequiered');
            }
        });

        var dropdownArticle = $('#articlesDrp .dropdown li a');
        dropdownArticle.on('click', function () {
            var articleValue = $(this).parent().attr('data-value');
            if (articleValue != null && articleValue != "") {
                var articleArr = articleValue.split('-');
                if (articleArr.length == 3) {
                    var artNo = articleArr[0];
                    var driedUndried = articleArr[1];
                    if (driedUndried.length >= 2) {
                        driedUndried = driedUndried.substring(0, 1).toUpperCase() + driedUndried.substring(1);
                    }

                    $('#agrementDriedUnDried').val(driedUndried);

                }
            }
        });
        $("#loadRadio").unbind("change").change(function () {
            searchItems("", $("#resourceDrp").attr("data-value"));
        });

        $("#unloadradio").unbind("change").change(function () {
            searchItems("", $("#resourceDrp").attr("data-value"));
        });

        $("#selectedDate").change(function () {
            searchItems("", $("#resourceDrp").attr("data-value"));
        });
    };

    var searchAvailableSlotsEvent = function () {
        $('#searchAvailbleSlotsBtn').on('click', function () {
            searchAvalibleSlotAction();
        });
    };

    var searchCustomerEvent = function () {
        $('#searcFieldhBtn').click(function () { searchCustomer(); });
    };

    var bokaSearchTypeChangeEvent = function () {
        $("#search-option > ul > li > a").click(function () {
            var placeholderData = $(this).html();
            $("#seachFieldTxt").attr('placeholder', "Ange " + placeholderData.toLowerCase());
        });
    }

    var searchCustomerWhenLoadPageEvent = function () {
        if ($('#seachFieldTxt').val() != '') {
            $("#searcFieldhBtn").removeClass("disabled-btn");
            searchCustomer();
        } else {
            $("#searcFieldhBtn").addClass("disabled-btn");
        }
    }

    var init = function () {
        searchItemEvent();
        validateRequiredFieldsEvent();
        searchAvailableSlotsEvent();
        makeReservationEvent();
        updateReservationEvent();
        searchCustomerEvent();
        bokaSearchTypeChangeEvent();
        searchCustomerWhenLoadPageEvent();
        validateButtonSearchEvent();
    }

    return {
        init: init,
        searchItems: searchItems,
        searchAvalibleSlotAction: searchAvalibleSlotAction
    }
};

var ListingBookingPage = function () {
    //search
    var doSearch = function () {
        blockUI();
        var fromDateValue = $('#fromDateTxt').val();
        var toDateValue = $('#toDateTxt').val();
        var customerNoValue = $('#customerNo').val();
        var regNoValue = $('#RegNo').val();
        var resourceGroupIdValue = $('#bl-resourceDrp').attr("data-value");
        var resourceType = '';
        var resourceNumber = '';
        var referenceType = '';
        var referenceNumber = '';
        $('#SaveTable').hide();
        $('#updateBtn').hide();

        var obj = new Object();
        obj.ResourceGroupId = resourceGroupIdValue;
        obj.FromDate = fromDateValue;
        obj.ToDate = toDateValue;
        obj.RegNo = regNoValue;
        obj.CustomerNo = customerNoValue;
        obj.ReferenceType = referenceType;
        obj.ReferenceNumber = resourceNumber;
        obj.PurchseOrderLine = referenceNumber;
        var containerDto = new Object();
        containerDto.request = obj;

        $.ajax({
            type: 'POST',
            url: '/api/listing-booking/SearchBookings',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(containerDto),

            success: function (response) {
                searchComplete(response);
                unBlockUI();
            },
            error: function (response) {
                alert('Error: ' + response);
                $("#search-result-book-listing").hide();
                unBlockUI();
            }
        });

        var searchComplete = function (response) {
            $("#search-result-book-listing").html(response);
            if (response.resultSearchBookings == undefined) {
                $("#search-result-book-listing").show();
            } else {
                $("#search-result-book-listing").hide();
            }
            deleteBookingEvents();
            editBookingEvent();
            printPageEvents();
            $(document).trigger("enhance.tablesaw");
        };
    };

    //delete
    var deleteBooking = function (deleteButton) {
        var dataValues = $(deleteButton).parent().parent();

        var reservationId = dataValues.find("> #reservationId > a").html();
        var reservationDate = dataValues.find("#from-date").val();
        var time = dataValues.find("#from-time").val();
        var owner = dataValues.find("> #owner").html();
        var customerNo = dataValues.find("> #customer-number").html();
        var arr = new Array();
        arr[arr.length] = '<p>';
        arr[arr.length] = '<span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>';

        arr[arr.length] = 'Vill du verkligen radera bokningen<br \/> kl: ' + time + '?';
        arr[arr.length] = '<\/p>';

        clearContentToDeleteDialog();
        fillContentToDeleteDialog("Ta bort bokning?", arr.join(''));
        $("#deleteDialog").find(".btn-ok").on("click", function () {
            deletReservationProcessList(reservationId, owner, customerNo, reservationDate, time);
        });

        var deletReservationProcessList = function (reservationId, owner, customerNo, reservationDate, time) {
            clearFormList();
            var o = new Object();
            o.reservationId = reservationId;
            o.owner = owner;
            o.customerNo = customerNo;
            o.dateRegistered = reservationDate + " " + time;
            $.ajax({
                type: 'POST',
                url: '/api/time-booking/DeleteReservation',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(o),

                success: function (response) { deletReservationSuccessList(response.deleteReservations); },
                error: function (response) { deletReservationErrorList(); }
            });

            var deletReservationSuccessList = function (response) {
                var arr = new Array();
                arr[arr.length] = '<p>';
                for (var i = 0; i < response.length; i++) {
                    if (response[i].Status == 500) {

                        arr[arr.length] = '<span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>';
                    }
                    arr[arr.length] = response[i].Message + '<br \/>';
                }
                arr[arr.length] = '<\/p>';

                clearContentToModal("#dialogArea");
                fillContentToModal("#dialogArea", "Borttagning", arr.join(''));
                $("#dialogArea").find(".btn-ok").on("click", function () {
                    doSearch();
                });

                $('#SaveTable').hide();
                $('#updateBtn').hide();
            }

            var deletReservationErrorList = function () {
                $('#SaveTable').hide();
                $('#updateBtn').hide();
                alert('Fel uppstod vid anropet till tjänsten.');
            }
        }

    }

    //edit
    var editBooking = function (editButton) {
        var copyValuesToEnableEditList = function (editOjb) {
            $('#myDisplayMessage').html(editOjb.displayMessage);
            $("div").data("dateRegisteredList", editOjb.dateRegistered);
            $('#SaveTable').show();
            $('#IDLevnr').val(editOjb.idLevnr);
            $('#IDReg').val(editOjb.idReg);
            $('#IDOvrigt').val(editOjb.idOvrigt);
            var reminder = $("#IDReminder");
            if (editOjb.idReminder === "0" || editOjb.idReminder === "") {
                reminder.attr("data-value", "");
                reminder.find(">a").html("Ingen påminnelse");
            } else {
                reminder.attr("data-value", editOjb.idReminder);
                var subReminder = reminder.find('ul > li[data-value="' + editOjb.idReminder + '"]');
                var valueSubReminder = subReminder.find("a").html();
                subReminder.addClass("selected");
                reminder.find(">a").html(valueSubReminder);
            }

            $('#IDEpost').val(editOjb.idEpost);
            $('#IDMobil').val(editOjb.idMobil);

            $('#saveAction').val('Update');
            $('#ReservationID').val(editOjb.reservationId);


            $('#SpeditorNo_ForUpdate').val(editOjb.speditorNo);

            $('#CustomerNo_ForUpdate').val(editOjb.customerNo);
            $('#CustomerName_ForUpdate').val(editOjb.customerName);

            $('#newCustomerNo').val(editOjb.customerNo);
            $('#newCustomerName').val(editOjb.customerName);

            $('#TransportOrderNo_ForUpdate').val(editOjb.transportOrderNo);
            $('#ContractNo_ForUpdate').val(editOjb.contractNo);
            $('#ReminderEmail_ForUpdate').val(editOjb.reminderEmail);
            $('#ReminderSMS_ForUpdate').val(editOjb.reminderSms);
            $('#Owner').val(editOjb.owner);


            $('#dispalyMessageHeader').html('Ändra bokning genom att ändra uppgifter nedan och klicka Uppdatera bokning.');
            if ($('#CustomerName_ForUpdate').val() != editOjb.customerName) {
                alert('fel');
            }

            $('#updateBtn').unbind('click');
            $('#updateBtn').click(function () { updateReservationList(); });
            $('#newCustomerNo').val(editOjb.customerNo);

            $('#updateBtn').show();
        }
        var updateReservationList = function () {
            var oldCustomerNo = $('#CustomerNo_ForUpdate').val();
            var oldCustomerName = $('#CustomerName_ForUpdate').val();

            var newCustomerNo = $('#newCustomerNo').val();
            var newCustomerName = $('#newCustomerName').val();
            var loadUnLoadVale = $('#loadOrUnlodRad').val();
            var mobileNo = $('#IDMobil').val();
            var leveransforsakransnummer = $('#IDLevnr').val();
            var note = $('#IDOvrigt').val();
            var reminderInMinutesBefore = $('#IDReminder').attr("data-value");
            var licensePlateNo = $('#IDReg').val();
            var email = $('#IDEpost').val();
            var owner = $('#Owner').val();
            var reservationId = $('#ReservationID').val();
            var speditor = $('#SpeditorNo_ForUpdate').val();

            if (validateSaveTable(email, mobileNo, licensePlateNo, leveransforsakransnummer)) {
                var innerDto = new Object();
                innerDto['Note'] = note;
                innerDto['ReminderInMinutesBefore'] = reminderInMinutesBefore;
                innerDto['EmailAddress'] = email;
                innerDto['NewCustomerName'] = newCustomerName;
                innerDto['NewCustomerNo'] = newCustomerNo;
                innerDto['OldCustomerName'] = oldCustomerName;
                innerDto['OldCustomerNo'] = oldCustomerNo;

                innerDto['Leveransforsakransnummer'] = leveransforsakransnummer;
                innerDto['LicensePlateNo'] = licensePlateNo;
                innerDto['MobilePhone'] = mobileNo;
                innerDto['ReminderInMinutesBefore'] = reminderInMinutesBefore;
                innerDto['EmailAddress'] = email;
                innerDto['SpeditorNo'] = speditor;

                innerDto['ReservationId'] = reservationId;
                innerDto['Owner'] = owner;
                innerDto['DateRegistered'] = $("div").data("dateRegisteredList");

                var containerDto = new Object();
                containerDto.reservationToUpdate = innerDto;
                $.ajax({
                    type: 'POST',
                    url: '/api/time-booking/UpdateReservation',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(containerDto),

                    success: function (response) {
                        updateReservationListSuccess(response.updateReservations);
                    },
                    error: function (response) { updateReservationListError(response.updateReservations); }
                });
            }
        }
        var updateReservationListSuccess = function (response) {
            var arr = new Array();

            arr[arr.length] = '<p>';
            for (var i = 0; i < response.length; i++) {

                if (response[i].Status == 200) {
                    arr[arr.length] = response[i].Message;
                } else {
                    arr[arr.length] = response[i].ErrorMessage;
                }
                arr[arr.length] = '<br \/>';
            }

            arr[arr.length] = '<\/p>';

            clearContentToModal("#dialogArea");
            fillContentToModal("#dialogArea", "Bokningen uppdaterad", arr.join(''));
            clearFormList();

            $('#SaveTable').hide();
            $('#updateBtn').hide();
            doSearch();
        }
        var updateReservationListError = function (response) {
            $('#SaveTable').hide();
            alert('updateReservationError');
            doSearch();
        }

        var editObj = new Object();
        var dataValues = $(editButton).parent().parent();
        editObj.resourceName = dataValues.find("#resourceName").val();
        editObj.itemName = dataValues.find("#itemName").val();
        editObj.weight = dataValues.find(">#weight").html();
        editObj.fromTime = dataValues.find("#from-time").val();
        editObj.itemNo = dataValues.find(">#itemNo").html();
        editObj.idLevnr = dataValues.find(">#leveransforsakransnr").html();
        editObj.idReg = dataValues.find(">#licensePlateNo").html();
        editObj.idOvrigt = dataValues.find("#note").val();
        editObj.idReminder = dataValues.find("#reminderMinutesBefore").val();
        editObj.idEpost = dataValues.find("#emailAddress").val();
        editObj.idMobil = dataValues.find("#mobileNo").val();
        editObj.reservationId = dataValues.find(">#reservationId > a").html();
        editObj.speditorNo = dataValues.find("#speditorNo").val();
        editObj.customerNo = dataValues.find(">#customer-number").html();
        editObj.customerName = dataValues.find("#customer-name").val();
        editObj.transportOrderNo = dataValues.find("#transportOrderNo").val();
        editObj.contractNo = dataValues.find("#contractNo").val();
        editObj.reminderEmail = dataValues.find("#reminderEmail").val();
        editObj.reminderSms = dataValues.find("#reminderSms").val();
        editObj.owner = dataValues.find(">#owner").html();
        editObj.dateRegistered = dataValues.find("#dateRegistered").val();
        editObj.displayMessage = 'Tid: ' + editObj.fromTime + ', Kund: ' + editObj.customerNo + ' ' + editObj.customerName + ', Bokningsnr: ' + editObj.reservationId + ', Artikel: ' + editObj.itemNo + ' ' + editObj.itemName + editObj.weight + '<br \/>Resurs: ' + editObj.resourceName;
        copyValuesToEnableEditList(editObj);
    }

    var clearFormList = function () {
        $('.fieldToClearWithClearForm').val('');
        $('#SaveTable').hide();
        $('#updateBtn').hide();

        $('#IDEpost').val($('#IDEpost_StartValue').val());
        $('#IDMobil').val($('#IDMobil_StartValue').val());
        $('#saveAction').val('New');
        //$('#saveBtn').val('Spara bokningen');


        $('#saveAction').val('New');
        $('#myDisplayMessage').html('');
        $('#dispalyMessageHeader').html('&nbsp;');
        //$('#saveBtn').button('disable');
    }

    var printPageEvents = function () {
        var printBtn = $('#printLink');
        $(printBtn).unbind("click");
        $(printBtn).click(function () {
            var blockSearchResult = $("#search-result-book-listing");
            var printWin = window.open();
            blockSearchResult.find("#printLink").hide();
            printWin.document.write(blockSearchResult.html());
            blockSearchResult.find("#printLink").show();
            printWin.print();
            printWin.close();
        });
    }

    var searchBookingEvents = function () {
        $('#search-booking-button').click(function (e) {
            doSearch();
            e.preventDefault();
        });
    }
    var deleteBookingEvents = function () {
        var deleteBtn = $(".boka-listing-deleteBtn");
        $(deleteBtn).unbind("click");
        $(deleteBtn).click(function (e) {
            deleteBooking($(this));
            e.preventDefault();
        });
    }
    var editBookingEvent = function () {
        var editBtn = $(".boka-listing-editBtn");
        $(editBtn).unbind("click");
        $(editBtn).click(function (e) {
            editBooking($(this));
            clearValidateSaveTable();
            e.preventDefault();
        });
    }

    var init = function () {
        searchBookingEvents();
    }

    return {
        init: init
    }
}

var validateSaveTable = function (email, phone, regnr, levnr) {
    var reEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    var rePhone = /^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$/;
    var reRegnr = /^([a-zA-Z]{3}\d{3})+$/;
    var reLevnr = /^\d+$/;
    var result = true;
    clearValidateSaveTable();

    if (email !== "" && !reEmail.test(email)) {
        result = false;
        $("#IDEpost").addClass("error");
        $("#IDEpost-error").show();
    }
    
    if (phone !== "" && !rePhone.test(phone)) {
        result = false;
        $("#IDMobil").addClass("error");
        $("#IDMobil-error").show();
    }
    if ($('#IDReg').hasClass('updateRequiered')) {
        if (!reRegnr.test(regnr)) {
            result = false;
            $("#IDReg").addClass("error");
            $("#IDReg-error").show();
        }
    }
    if (!reLevnr.test(levnr)) {
        result = false;
        $("#IDLevnr").addClass("error");
        $("#IDLevnr-error").show();
    }
    return result;
}

var clearValidateSaveTable = function() {
    $("#IDEpost-error").hide();
    $("#IDMobil-error").hide();
    $("#IDLevnr-error").hide();
    $("#IDReg-error").hide();
    $("#IDEpost").removeClass("error");
    $("#IDMobil").removeClass("error");
    $("#IDReg").removeClass("error");
    $("#IDLevnr").removeClass("error");
}

jQuery(function () {
    CustomerCard.init();
});

var CustomerCard = CustomerCard || (function () {
    var customerNumber = "";
    var errorMessages = {
        delError: "",
        updateError: ""
    }
    function addProfileHandler() {
        var $userProfile = $("a.user-profile");
        $userProfile.each(function (idx, item) {
            $(item).click(function () {
                var selectedProfileId = $(item).closest("li.showcase").attr("data-value");
                var userName = $(item).attr("data-userName");
                var loader = $(item).closest("td").find(".loader-wrapper");
                var confirmPanel = $(item).closest("td").find(".confirm-panel .confirm-box");
                if (confirmPanel.length > 0) {
                    var $confirmPanel = $(confirmPanel);
                    $confirmPanel.show();
                    $("input[type=reset]", $confirmPanel).click(function () {
                        $confirmPanel.hide();
                    });
                    $("input[type=button]", $confirmPanel).click(function () {
                        showLoader(loader);
                        changeProfileForUser(userName, selectedProfileId, customerNumber,
                            function () { $confirmPanel.hide(); },
                            function() {
                                showErrorDialog(errorMessages.updateError); 
                            },
                            function() {
                                hideLoader(loader);
                                $confirmPanel.hide();
                            });
                    });
                }
            });
        });

        $(".fa.fa-trash-2").click(function () {
            var userName = $(this).attr("data-userName");
            var userId = $(this).attr("data-userId");
            var userElement = $(this).closest("tr");
            var loader = $(userElement).find(".loader-wrapper");
            var $confirmDialog = $("#dg-del-user-confirmation");
            if ($confirmDialog.length > 0) {
                $confirmDialog.fadeIn();
                // event for confirmation dialog
                $("button.yes").click(function() {
                    showLoader(loader);
                    removeUserFromCustomer(userName, userId, customerNumber,
                        function() {
                             $(userElement).remove();
                        },
                        function () { showErrorDialog(errorMessages.delError); },
                        function() {
                            hideLoader(loader);
                        });
                });
            }
        });
    }
    

    function removeUserFromCustomer(userName, userId, customerNumber, successCallBack, failCallBack, completeCallBack) {
        $.ajax({
            url: '/api/customer-card/remove-user?userName=' + userName + "&userId=" + userId + "&customerNo=" + customerNumber,
            type: "GET",
            dataType: "json",
            success: function (data) {
                if (data) {
                    successCallBack();
                } else {
                    failCallBack();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                failCallBack();
            },
            complete:function() {
                completeCallBack();
            }
        });
    }
    function showLoader(loader) {
        if (loader != undefined) {
            $(loader).show();
        }
    }

    function hideLoader(loader) {
        if (loader != undefined) {
            $(loader).hide();
        }
    }

    function changeProfileForUser(userName, profileId, customerNo, successCallBack, failCallBack, completeCallBack) {
        $.ajax({
            url: '/api/customer-card/change-profile?userName=' + userName + "&profileId=" + profileId + "&customerNo=" + customerNo,
            type: "GET",
            dataType: "json",
            success: function (data) {
                if (data) {
                    successCallBack();
                } else {
                    failCallBack();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                failCallBack();
            },
            complete: function () {
                completeCallBack();
            }
        });
    }

    function showErrorDialog(errorMess) {

    }

    var init = function () {
        customerNumber = $("input#customer-no") ? $("input#customer-no").val() : "";
        addProfileHandler();
    }
    return {
        init: init
    }
})();



/*------------------------------------*\
    #INTERNAL-NAVIGATION
\*------------------------------------*/


window.gro = window.gro || {};
gro.modules = gro.modules || {};


gro.modules.internalNavigation = (function () {
    var priv = {};

    priv.toggleNavigation = function(parentNav, isToggleClicked) {
        var isToggleClicked = false || isToggleClicked;
        // check navigation expanded or not
        if (!parentNav.hasClass("expanded")) {
            parentNav.addClass("expanded");
        } else {
            if (!isToggleClicked) parentNav.removeClass("expanded");
        }
    };

    /**
     * Private initialization method
     */
    priv.init = function () {

        /**
         * Click on category link
         * Link should only expand child list, not be followed
         * Fallback (href on the <a> tag) should be the first page in the child list
         */
        $(".js-internal-navigation-category").on("click", function (e) {
            e.preventDefault();
            var control = this;
            var target = $(control).next(".internal-navigation__list--sub-level");
            var parentNav = $(control).parents(".internal-navigation");

            if ($(control).hasClass("is-expanded")) {
                $(target).slideUp(function () {
                    $(control).removeClass("is-expanded");
                    $(target)
                        .removeClass("is-expanded")
                        .removeAttr("style");
                });
            } else {
                $(control)
                    .addClass("is-expanded");
                $(target).slideDown(function () {
                    $(target)
                        .addClass("is-expanded")
                        .removeAttr("style");
                });
            }

            /*
             * Expand navigation if end users click any links which have sub-navigation inside
             */
            priv.toggleNavigation(parentNav, true);
        });

        /**
         * Click on toggle link
         * Link should toggle whole navigation
         */
        $(".internal-navigation__toggle-btn").on("click",
            function(e) {
                e.preventDefault();
                var control = this;
                var target = $(control).parent().find(".internal-navigation__list--sub-level");
                var parentNav = $(control).parents(".internal-navigation");

                /*
                 * Toggle navigation if end users click on toggle btn
                 */
                priv.toggleNavigation(parentNav);

                /*
                 * Collapse all opened sub-navigation when navigation collapsed
                 */
                $(target).slideUp(function () {
                    $(this).prev().removeClass("is-expanded");
                    $(this)
                        .removeClass("is-expanded")
                        .removeAttr("style");
                });
            });
    };

    // Initialize module
    $(function () {
        priv.init();
    });

})();
