using Microsoft.AspNetCore.Mvc;
using MSToolKit.Mvc.Enums;
using System.Collections.Generic;

namespace MSToolKit.Mvc.Abstraction
{
    /// <summary>
    /// Extends Microsoft.AspNetCore.Mvc.Controller 
    /// and provides methods that return MSToolKit.Mvc.ExcelResult.
    /// </summary>
    public abstract class ExcelController : Controller
    {
        /// <summary>
        /// Returns new instance of MSToolKit.Mvc.ExcelResult.
        /// </summary>
        /// <param name="data">
        /// The bytes that should be attached to the http response as a excel file.
        /// </param>
        /// <returns>New instance of MSToolKit.Mvc.ExcelResult.</returns>
        protected ExcelResult Excel(IEnumerable<byte> data) 
            => new ExcelResult(data);

        /// <summary>
        /// Returns new instance of MSToolKit.Mvc.ExcelResult.
        /// </summary>
        /// <param name="data">
        /// The bytes that should be attached to the http response as a excel file.
        /// </param>
        /// <param name="fileName">
        /// The name, which should be used for the attached file.
        /// </param>
        /// <returns>
        /// New instance of MSToolKit.Mvc.ExcelResult.
        /// </returns>
        protected ExcelResult Excel(IEnumerable<byte> data, string fileName) 
            => new ExcelResult(data, fileName);

        /// <summary>
        /// Returns new instance of MSToolKit.Mvc.ExcelResult.
        /// </summary>
        /// <param name="data">
        /// The bytes that should be attached to the http response as a excel file.
        /// </param>
        /// <param name="fileName">
        /// The name, which should be used for the attached file.
        /// </param>
        /// <param name="fileExtension">
        /// The extension, that should be used for the attached file.
        /// </param>
        /// <returns>
        /// New instance of MSToolKit.Mvc.ExcelResult.
        /// </returns>
        protected ExcelResult Excel(
            IEnumerable<byte> data, string fileName, ExcelFileExtension fileExtension)
                => new ExcelResult(data, fileName, fileExtension);
    }
}
