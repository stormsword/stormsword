# Build script for Stormsword
# Unity Command Line Arguments: http://docs.unity3d.com/Manual/CommandLineArguments.html

# Usage:
# make version=0.1
# Places win/osx/linux builds into $(build_path)/$(version)
#
# Remove directories to re-build:
# make clean version=0.1
#

# Version (required) - Sets the version for the current build
# 		Used by Github in managing releases
version?=$(error Required variable `version` is missing.)

all: clean osx windows linux

windows:
	$(unity_path) $(batchmode) $(nographics) $(quit_when_done) -buildWindows64Player $(build_path)/$(version)/Stormsword.v$(version).windows.exe
	cd $(build_path)/$(version) && zip -rmX Stormsword.v$(version).windows.zip Stormsword.v$(version).windows*

osx:
	$(unity_path) $(batchmode) $(nographics) $(quit_when_done) -buildOSX64Player $(build_path)/$(version)/Stormsword.v$(version).osx
	cd $(build_path)/$(version) && zip -rmX Stormsword.v$(version).osx.zip Stormsword.v$(version).osx*

linux:
	$(unity_path) $(batchmode) $(nographics) $(quit_when_done) -buildLinux64Player $(build_path)/$(version)/Stormsword.v$(version).linux
	cd $(build_path)/$(version) && zip -rmX Stormsword.v$(version).linux.zip Stormsword.v$(version).linux Stormsword.v$(version)_Data*

clean:
	rm -rf $(build_path)/$(version)
	mkdir $(build_path)/$(version)

unity_path = /Applications/Unity/Unity.app/Contents/MacOS/Unity
batchmode = -batchmode
nographics = -nographics
quit_when_done = -quit
build_path = ~/Desktop/Stormsword