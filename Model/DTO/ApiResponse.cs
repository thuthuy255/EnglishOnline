using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }  // Trạng thái thành công
        public T Data { get; set; }        // Dữ liệu trả về
        public string Message { get; set; } // Thông báo hoặc lỗi

        // Constructor
        public ApiResponse(bool success, T data, string message = "")
        {
            Success = success;
            Data = data;
            Message = message;
        }
    }
}
