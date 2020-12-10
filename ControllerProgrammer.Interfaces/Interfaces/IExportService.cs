using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DevExpress.XtraPrinting;


namespace ControllerProgrammer.Common.Interfaces {
    public enum ExportFormat { Xlsx,Pdf,Csv }

    public interface IExportService {
        void Export(Stream stream, ExportFormat format, XlsxExportOptionsEx options = null);
    }
}
