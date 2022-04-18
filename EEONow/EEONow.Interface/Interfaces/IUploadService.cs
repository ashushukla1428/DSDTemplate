using System;
using System.Collections.Generic;
using System.Data;
using EEONow.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEONow.Interface
{
    public interface IUploadService
    { 
        List<FileSubmissionRecordsModel> GetFileSubmissionRecords();
        List<ValidateEmployeeRecords> GetNotValidateFileSubmissionRecords();
        List<EmployeeErrorlist> BindEmployeeSubmissionErrorList();

        
        String GetFileName();
        ResponseModel CsvUploading(String FileName, String OrignalName,String NewFileName,DateTime SubmissionDateTime);

       Task<ResponseModel> DeleteFileSubmissionRecords();

        ResponseModel ValidateEmployeeRecords();

        Task<ResponseModel> UpdateFileSubmission(ValidateEmployeeRecords _EmployeeRecords);

        FileVersionClass BindFileSubmissionToolbar();

    }
}
