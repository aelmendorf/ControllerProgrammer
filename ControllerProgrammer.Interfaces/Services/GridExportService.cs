using ControllerProgrammer.Common.Interfaces;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ControllerProgrammer.Common.Services {
    public class GridExportService : ServiceBase, IExportService {
        public void Export(Stream stream, Interfaces.ExportFormat format, XlsxExportOptionsEx options = null) {
            GridControl grid = (GridControl)AssociatedObject;

            switch (format) {
                case Interfaces.ExportFormat.Xlsx: {
                        if (options != null) {
                            grid.View.ExportToXlsx(stream, options);
                        } else {
                            grid.View.ExportToXlsx(stream);
                        }
                        break;
                    }
                case Interfaces.ExportFormat.Pdf: {
                        grid.View.ExportToPdf(stream);
                        break;
                    }
                case Interfaces.ExportFormat.Csv: {
                        grid.View.ExportToCsv(stream);
                        break;
                    }
            }
        }
    }

}
