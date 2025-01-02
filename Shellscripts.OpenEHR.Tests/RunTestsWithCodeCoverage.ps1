Clear-Host

# 1. Clear the Test Results folder
$folderPath = "./TestResults"
if (Test-Path -Path $folderPath) {
    # Get all files and subdirectories in the folder
    $items = Get-ChildItem -Path $folderPath
    
    # Remove all items (files and subfolders)
    foreach ($item in $items) {
        Remove-Item -Path $item.FullName -Recurse -Force -ErrorAction SilentlyContinue
    }
    Write-Host "Contents of the folder '$folderPath' have been cleared." -ForegroundColor Red
} 
else {
    Write-Host "The folder '$folderPath' does not exist." -BackgroundColor Red -ForegroundColor White
}

# 2. Run the Tests with Code Coverage Enabled and get the output
dotnet test --collect:"XPlat Code Coverage"

# 3. Get the fullname of the code coverage file and execute the Report Generator
$attachmentFile = (Get-ChildItem -Path "./TestResults" -Recurse -File -Filter "coverage.cobertura.xml").FullName
Write-Host "`nExtracted Path`t: $attachmentFile"

$reportGeneratorExpression = "reportgenerator -reports:""$attachmentFile"" -targetdir:"".\TestResults\CodeCoverage"" -reporttypes:html"
Invoke-Expression $reportGeneratorExpression -ErrorAction SilentlyContinue
write-Host "Completed Report Generation" -ForegroundColor Green