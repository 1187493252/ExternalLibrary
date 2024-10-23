mergeInto(LibraryManager.library, {
    AlertInfo:function (str) {
        window.alert(Pointer_stringify(str));
    },
    ExitGame:function (){
      window.open("about:blank","_self").close();
    },
    OpenCustomUrl (str){
      window.open(str);
    },
});