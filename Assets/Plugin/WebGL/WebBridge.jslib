// === ĐÂY LÀ FILE TOÀN BỘ LÀ MỚI NÊN BẠN CỨ COPY VÀO ===
mergeInto(LibraryManager.library, {
    SyncDataToWeb: function(jsonDataString) {
        // Dịch cục dữ liệu C# sang chuẩn của Javascript
        var parsedString = UTF8ToString(jsonDataString);
        
        // Gọi hàm ReceiveDataFromUnity nằm trên trang HTML của bạn và vứt data cho nó
        if (typeof window.ReceiveDataFromUnity === "function") {
            window.ReceiveDataFromUnity(parsedString);
        } else {
            console.warn("Chưa tạo hàm ReceiveDataFromUnity trên Website!");
        }
    }
});