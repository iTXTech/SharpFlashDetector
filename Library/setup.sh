#!/bin/sh

rm -rf sf
rm -rf sftemp
rm -rf FlashDetector
rm -rf fd.php
git clone https://github.com/iTXTech/SimpleFramework.git --depth 1 sftemp
export SF_HOME=$(pwd)/sftemp
git clone https://github.com/iTXTech/SimpleFramework.git -b peachpie --depth 1 sf
git clone https://github.com/iTXTech/FlashDetector.git --depth 1 fd
rm -rf sf/examples
rm -rf sf/.git
rm -rf sf/.gitignore sf/LICENSE sf/README.md sf/sf sf/sf.cmd
cd fd/PeachPie
php generate.php
cp -r FlashDetector ../../FlashDetector/
cp fd.php ../../
cd ../..
rm -rf fd
rm -rf sftemp
echo Done!
