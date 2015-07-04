namespace MtHoodMiata.Web
{
    public enum UploadResult
    {
        None,
        UploadSuccess,
        UploadAndEmailSuccess,
        FileExists,
        OnlyPdf,
        NoFileUploaded
    }
}