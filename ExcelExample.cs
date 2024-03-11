using NPOI.XSSF.UserModel;

namespace MAI.CadDev.Examples;

public static class ExcelExample
{
    public static void Do()
    {
        using (var stream = File.OpenRead("data.xlsx"))
        {
            using (var xssWorkbook = new XSSFWorkbook(stream))
            {
                var sheet = xssWorkbook.GetSheetAt(0);

                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    short minColIx = row.FirstCellNum;
                    short maxColIx = row.LastCellNum;

                    for (short colIx = minColIx; colIx < maxColIx; colIx++)
                    {
                        var cell = row.GetCell(colIx);
                        var value = cell.StringCellValue;
                        Console.WriteLine($"{i},{colIx}: {value}");
                    }
                }
            }
        }
    }
}
