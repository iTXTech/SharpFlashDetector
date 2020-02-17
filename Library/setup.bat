@echo off
rd /s /q sf
rd /s /q FlashDetector
del /q fd.php
git clone https://github.com/iTXTech/SimpleFramework.git -b peachpie --depth 1 sf
git clone https://github.com/iTXTech/FlashDetector.git --depth 1 fd
rd /s /q sf\examples
rd /s /q sf\.git
del /q sf\.gitignore sf\LICENSE sf\README.md sf\sf sf\sf.cmd
cd fd\PeachPie
php generate.php
xcopy /y /e FlashDetector ..\..\FlashDetector\
copy fd.php ..\..\
cd ..\..
rd /s /q fd
