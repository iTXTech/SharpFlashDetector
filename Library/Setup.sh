export SF_HOME = ./sf
rm -R sf
rm -R FlashDectector
rm fd.php
git clone https://github.com/iTXTech/SimpleFramework.git -b peachpie --depth 1 sf
git clone https://github.com/iTXTech/FlashDetector.git --depth 1 fd
rm -R sf/examples
rm -R sf/.git
rm sf/.gitignore sf/LICENSE sf/README.md sf/sf sf/sf.cmd
cd fd/PeachPie
php generate.php
cp FlashDetector ../../FlashDetector/
cp fd.php ../../
cd ../..
rm -R fd
echo Done!