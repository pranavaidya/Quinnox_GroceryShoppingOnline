namespace GroceryOnlineShopModelLibrary.DTOs
{
    public class GeneralResponseDto<T>
    {
        public bool Success { get; set; }
        public T? Value { get; set; }
        public string ErrorMessage { get; private set; } = string.Empty;

        public GeneralResponseDto(bool success, T value)
        {
            Success = success;
            Value = value;
        }

        public GeneralResponseDto()
        {
            Success = true;
            Value = default(T);
        }

        public GeneralResponseDto<T> Error(string errorDescriptor)
        {
            Value = default(T);
            Success = false;
            ErrorMessage = errorDescriptor;
            return this;
        }

        public GeneralResponseDto<T> Content(T? contentObject)
        {
            Value = contentObject;
            Success = true;
            ErrorMessage = string.Empty;
            return this;
        }
    }
}
