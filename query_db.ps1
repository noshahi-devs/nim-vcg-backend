$cs = 'Server=93.127.141.27,14330;Database=SchoolSystemDb;User Id=sa;Password=Noshahi@000;TrustServerCertificate=True;'
$conn = New-Object System.Data.SqlClient.SqlConnection($cs)
try {
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Student' ORDER BY COLUMN_NAME"
    $r = $cmd.ExecuteReader()
    while($r.Read()){ 
        Write-Host $r[0] 
    }
} finally {
    $conn.Close()
}
