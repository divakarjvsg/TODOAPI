namespace ToDoAPI.Models.ResponseModels
{
    public class ApiResponse
    {
        /// <summary>
        /// Error Message
        /// </summary>
        /// <example>Not Found</example>
        public string Message { get; set; }
    }

    public class status404error
    {
        /// <summary>
        /// Not Found
        /// </summary>
        /// <example>Not Found</example>
        public string title { get; set; } = "NotFound";
        /// <summary>
        /// 404
        /// </summary>
        /// <example>404</example>
        public int Status { get; set; } = 404;
        /// <summary>
        /// traceId
        /// </summary>
        /// <example>eb9867b3-418723db288fd415</example>
        public string traceId{ get; set; }
    }

    public class status400error
    {
        /// <summary>
        /// Bad Request
        /// </summary>
        /// <example>one or more validaton errors occurred</example>
        public string title { get; set; } 
        /// <summary>
        /// 400
        /// </summary>
        /// <example>400</example>
        public int Status { get; set; }
        /// <summary>
        /// traceId
        /// </summary>
        /// <example>eb9867b3-418723db288fd415</example>
        public string traceId { get; set; }
    }

    public class patchtodoitem404
    {
        /// <summary>
        /// Error Message
        /// </summary>
        /// <example>Item with Id not found</example>
        public string Message { get; set; }
    }

    public class itemidmismatch
    {
        /// <summary>
        /// Error Message
        /// </summary>
        /// <example>Item ID mismatch</example>
        public string Message { get; set; }
    }
    public class invaliditem
    {
        /// <summary>
        /// Error Message
        /// </summary>
        /// <example>Invalid Id</example>
        public string Message { get; set; }
    }
}
