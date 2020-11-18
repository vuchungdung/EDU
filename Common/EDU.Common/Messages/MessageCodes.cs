namespace EDU.Common.Message
{
    public class MessageCodes
    {
        public static string ArgumentExceptionData = "MESSAGE.ArgumentData";
        public static string NotSupportedContentType = "MESSAGE.NotSupportedContentType";
        public static string NotImplementMethod = "MESSAGE.ArgumentMethod";
        public static string ArgumentException = "MESSAGE.ArgumentException";
        public static string CreateSuccessfully = "MESSAGE.create_successfully";
        public static string CreateUploadSuccessfully = "MESSAGE.create_upload_successfully";
        public static string CreateFail = "MESSAGE.create_fail";
        public static string CreateUploadFail = "MESSAGE.create_upload_fail";
        public static string CreateRefillFail = "MESSAGE.create_refill_fail";
        public static string CrudSuccessfully = "MESSAGE.crud_successfully";
        public static string CrudFail = "MESSAGE.crud_fail";
        public static string UpdateSuccessfully = "MESSAGE.update_successfully";
        public static string ResetSucessfully = "MESSAGE.reset_successfully";
        public static string CancelSuccessfully = "MESSAGE.cancel_successfully";
        public static string CancelFail = "MESSAGE.cancel_fail";
        public static string VerifySuccessfully = "MESSAGE.verify_successfully";
        public static string VerifyFail = "MESSAGE.verify_fail";
        public static string UpdateFail = "MESSAGE.update_fail";
        public static string DeleteSuccessfully = "MESSAGE.delete_successfully";
        public static string DeleteFail = "MESSAGE.delete_fail";
        public static string CodeExists = "MESSAGE.code_exist";
        public static string InvalidInput = "MESSAGE.invalid_input";
        public static string SendSuccess = "MESSAGE.send_success";
        public static string SendFail = "MESSAGE.send_fail";
        public static string SendNotificationSuccess = "MESSAGE.send_notification_success";
        public static string SendNotificationFail = "MESSAGE.send_notification_fail";
        public static string CollectedSuccess = "MESSAGE.collected_success";
        public static string OutstandingSuccess = "MESSAGE.outstanding_success";
        public static string CollectedFail = "MESSAGE.collected_fail";

        public static string UsernameExists = "MESSAGE.user_name_exist";
        public static string CurrentPasswordIncorrect = "MESSAGE.current_password_incorrect";
        public static string TokenIncorrect = "MESSAGE.token_incorrect";

        public const string InvalidAppSetting = "MESSAGE.ConfigInvalid";
        public const string ExportSuccess = "MESSAGE.export_success";
        public const string ExportFail = "MESSAGE.export_fail"; 
    }
}