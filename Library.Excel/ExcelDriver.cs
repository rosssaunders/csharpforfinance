using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Library.Binominal;
using Library.Data;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace Library.Excel
{
    public class ExcelDriver
    {
        private readonly ExcelPackage _pExcel;
        private long _curDataColumn;

        //constructor
        public ExcelDriver(string file)
        {
            var fileInfo = new FileInfo(file);

            _pExcel = new ExcelPackage(fileInfo);
            _curDataColumn = 1;
        }

        public void Save()
        {
            _pExcel.Save();
            _pExcel.Dispose();
        }

        // Writes label and vector to cells in horizontal direction.
        private void ToSheetHorizontal<T>(ExcelWorksheet pWorksheet, int sheetRow, int sheetColumn, string label, Vector<T> values)
        {
            // First cell contains the label.
            var item = pWorksheet.Cells[sheetRow, sheetColumn];
            item.Value = label;
            sheetColumn++;

            // Next cells contain the values.
            for (var i = values.MinIndex; i <= values.MaxIndex; i++)
            {
                item = pWorksheet.Cells[sheetRow, sheetColumn];
                item.Value = values[i];
                sheetColumn++;
            }
        }

        // Writes label and vector to cells in vertical direction.
        private void ToSheetVertical<T>(ExcelWorksheet pWorksheet, int sheetRow, int sheetColumn, string label, Vector<T> values)
        {
            // First cell contains the label.
            var item = pWorksheet.Cells[sheetRow, sheetColumn];
            item.Value = label;
            sheetRow++;

            // Next cells contain the values.
            for (var i = values.MinIndex; i <= values.MaxIndex; i++)
            {
                item = pWorksheet.Cells[sheetRow, sheetColumn];
                item.Value = values[i];
                sheetRow++;
            }
        }

        // Returns row from matrix as vector.
        private Vector<T> createVector<T>(Matrix<T> mat, int row)
        {
            var nCols = mat.Columns;

            var result = new Vector<T>(nCols, mat.MinColumnIndex);
            for (var i = mat.MinColumnIndex; i <= mat.MaxColumnIndex; i++)
            {
                result[i] = mat[row, i];
            }

            return result;
        }


        // Create chart with a number of functions. The arguments are:
        //  x:			vector with input values
        //  labels:		labels for output values
        //  vectorList: list of vectors with output values.
        //  chartTitle:	title of chart
        //  xTitle:		label of x axis
        //  yTitle:		label of y axis
        public void CreateChart<T>(Vector<T> x, List<String> labels, List<Vector<T>> vectorList, string chartTitle, string xTitle, string yTitle)
        {
            //try
            //{
            //    if (labels.Count != vectorList.Count)
            //    {
            //        throw (new IndexOutOfRangeException("Number of labels must equal number of vectors."));
            //    }

                /*
                //create workbook & sheet
                Excel.Workbook pWorkbook = ( Excel.Workbook )InvokeMethodInternational( pExcel.Workbooks, "Add", XlWBATemplate.xlWBATWorksheet );
                Excel.Sheets pSheets = pWorkbook.Worksheets;

                Excel.Worksheet pWorksheet = ( Excel.Worksheet )pWorkbook.ActiveSheet;
                pWorksheet.Name = "Data";
                */

                // create (workbook &) sheet
                //ExcelWorkbook pWorkbook;
                //ExcelWorksheet pWorksheet;
                //if (pExcel.ActiveWorkbook == null)
                //{
                //    pWorkbook = (Excel.Workbook)InvokeMethodInternational(pExcel.Workbooks, "Add", Excel.XlWBATemplate.xlWBATWorksheet);
                //    pWorksheet = (Excel.Worksheet)pWorkbook.ActiveSheet;
                //    pWorksheet.Name = "Data";
                //}
                //else
                //{
                //    pWorkbook = pExcel.ActiveWorkbook;
                //    pWorksheet = (Excel.Worksheet)pWorkbook.Worksheets.get_Item("Data");
                //    //pWorksheet = ( Excel.Worksheet )InvokeMethodInternational( pWorkbook.Worksheets, "Add", Type.Missing, Type.Missing, 1, Type.Missing );
                //}

                //// Calculate range with source data.
                //// The first row contains the labels shown in the chart's legend.
                //long beginRow = 1;
                //long beginColumn = curDataColumn;
                //long endRow = x.MaxIndex + 2;                           // +1 to include labels.
                //long endColumn = beginColumn + vectorList.Count;        // 1st is input, rest is output.

                //// Write label + input values to cells in vertical direction.
                //ToSheetVertical(pWorksheet, 1, curDataColumn, chartTitle, x);
                //curDataColumn++;

                //// Write label + output values to cells in vertical direction.
                //for (int i = 0; i != vectorList.Count; i++)
                //{
                //    // Get label and row index.
                //    string label = labels[i];

                //    // Add label + output values to Excel.
                //    ToSheetVertical(pWorksheet, 1, curDataColumn, label, vectorList[i]);

                //    // Advance row.
                //    curDataColumn++;
                //}

                //// Create range objects for source data.
                //ExcelRange pBeginRange = (ExcelRange)pWorksheet.Cells[beginRow, beginColumn];
                //ExcelRange pEndRange = (ExcelRange)pWorksheet.Cells[endRow, endColumn];
                //ExcelRange pTotalRange = (ExcelRange)pWorksheet.get_Range(pBeginRange, pEndRange);

                //// Create the chart
                //ExcelChart pChart = (ExcelChart)InvokeMethodInternational(pWorkbook.Charts, "Add", pWorksheet, Type.Missing, 1, Type.Missing);
                ////pChart.Name = chartTitle;

                //InvokeMethodInternational(pChart, "ChartWizard", pTotalRange, Excel.XlChartType.xlXYScatter, 6, Excel.XlRowCol.xlColumns, 1, 1, true, chartTitle, xTitle, yTitle, "");
                //curDataColumn++;

                //pExcel.Visible = true;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Argument: " + e);
            //    try
            //    {
            //        _pExcel.Quit();
            //    }
            //    catch (COMException)
            //    {
            //        Console.WriteLine("User closed Excel manually, so we don't have to do that");
            //    }
            //}
        }

        // Create chart with an single function
        public void CreateChart<T>(Vector<T> x, Vector<T> y, string chartTitle, string xTitle, string yTitle)
        {
            // Create list with single function and single label.
            List<Vector<T>> functions = new List<Vector<T>>();
            List<string> labels = new List<string>();
            functions.Add(y);
            labels.Add(chartTitle);
            CreateChart(x, labels, functions, chartTitle, xTitle, yTitle);
        }

        // Add Matrix to the spreadsheet with row and column labels.
        public void AddMatrix<T>(string name, NumericMatrix<T> matrix, List<string> rowLabels, List<string> columnLabels)
        {
            try
            {
                // Check label count vs. matrix.
                if (matrix.Columns != columnLabels.Count)
                {
                    throw (new IndexOutOfRangeException("Count mismatch between # matrix columns and # column labels"));
                }
                if (matrix.Rows != rowLabels.Count)
                {
                    throw (new IndexOutOfRangeException("Count mismatch between # matrix rows and # row labels"));
                }

                // Add sheet
                ExcelWorksheet pSheet = _pExcel.Workbook.Worksheets.Add(name);

                // Current indeces in spreadsheet.
                int sheetRow = 1;
                int sheetColumn = 1;

                // Add column labels starting at column 2.
                sheetColumn = 2;
                ExcelRange pRange = pSheet.Cells;
                for (int i = 0; i != columnLabels.Count; i++)
                {
                    ExcelRange item = pSheet.Cells[sheetRow, sheetColumn];
                    item.Value = columnLabels[i];
                    sheetColumn++;
                }

                // Add row labels + values.
                sheetColumn = 1;
                sheetRow = 2;
                for (int i = matrix.MinRowIndex; i <= matrix.MaxRowIndex; i++)
                {
                    Vector<T> row = createVector(matrix, i);
                    ToSheetHorizontal(pSheet, sheetRow, sheetColumn, rowLabels[i - matrix.MinRowIndex], row);
                    sheetRow++;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Exception: " + e);
            }
        }

        // Add Lattice to the spreadsheet with row and column labels.
        public void AddLattice(string name, Lattice<double> lattice, List<string> rowLabels)
        {
            try
            {
                // Check label count vs. matrix.
                /*     if (lattice.Columns != columnLabels.Count)
                     {
                         throw (new IndexOutOfRangeException("Count mismatch between # matrix columns and # column labels"));
                     }
                     if (lattice.Rows != rowLabels.Count)
                     {
                         throw (new IndexOutOfRangeException("Count mismatch between # matrix rows and # row labels"));
                     }
                     */

                // Add sheet.
                ExcelWorksheet pSheet = _pExcel.Workbook.Worksheets.Add(name);
                
                // Add row labels + values.
                int sheetColumn = 1;
                int sheetRow = 1;
                for (int i = lattice.MinIndex; i <= lattice.MaxIndex; i++)
                {
                    Vector<double> row = lattice.PyramidVector(i);
                    ToSheetHorizontal<double>(pSheet, sheetRow, sheetColumn, rowLabels[i - lattice.MinIndex], row);
                    sheetRow++;
                }

                for (int i = lattice.MinIndex; i <= lattice.MaxIndex; i++)
                {
                    Vector<double> row = lattice.PyramidVector(i);
                    ToSheetHorizontal<double>(pSheet, sheetRow, sheetColumn, rowLabels[i], row);
                    sheetRow++;
                    // sheetColumn++;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Exception: " + e);
            }
        }
    }
}
