; 该脚本使用 HM VNISEdit 脚本编辑器向导产生

; 安装程序初始定义常量
!define PRODUCT_NAME "恒馨收银系统"
!define PRODUCT_VERSION "1.0.3_正式版"
!define PRODUCT_PUBLISHER "小伙工作室"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"
!define PRODUCT_STARTMENU_REGVAL "NSIS:StartMenuDir"

SetCompressor lzma

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI.nsh"

; MUI 预定义常量
!define MUI_ABORTWARNING
!define MUI_ICON "SuperCollectingSilver\form.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 安装目录选择页面
!insertmacro MUI_PAGE_DIRECTORY
; 开始菜单设置页面
var ICONS_GROUP
!define MUI_STARTMENUPAGE_NODISABLE
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "恒馨收银系统"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "${PRODUCT_UNINST_ROOT_KEY}"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "${PRODUCT_STARTMENU_REGVAL}"
!insertmacro MUI_PAGE_STARTMENU Application $ICONS_GROUP
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES
; 安装完成页面
!insertmacro MUI_PAGE_FINISH

; 安装卸载过程页面
!insertmacro MUI_UNPAGE_INSTFILES

; 安装界面包含的语言设置
!insertmacro MUI_LANGUAGE "SimpChinese"

; 安装预释放文件
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI 现代界面定义结束 ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "恒馨收银系统_${PRODUCT_VERSION}.exe"
InstallDir "$PROGRAMFILES\恒馨收银系统"
ShowInstDetails show
ShowUnInstDetails show

Section "MainSection" SEC01
  SetOutPath "$INSTDIR\*.*"
  SetOverwrite ifnewer
  File /r "SuperCollectingSilver\bin\x86\Debug\*.*"

; 创建开始菜单快捷方式
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd

Section -AdditionalIcons
  SetOutPath $INSTDIR
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
  CreateDirectory "$SMPROGRAMS\$ICONS_GROUP"
  /*******添加快捷方式--start*******/
  CreateShortCut "$DESKTOP\${PRODUCT_NAME}.lnk" "$INSTDIR\恒馨收银系统.exe"
  CreateShortCut "$SMPROGRAMS\$ICONS_GROUP\${PRODUCT_NAME}.lnk" "$INSTDIR\恒馨收银系统.exe"
  CreateShortCut "$SMPROGRAMS\$ICONS_GROUP\卸载.lnk" "$INSTDIR\uninst.exe"
  /*******添加快捷方式--end*******/
  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
  /*******添加用户注册表--start*******/
  WriteRegStr HKCU "Software\Microsoft\Internet Explorer\PageSetup" "footer" ""
  WriteRegStr HKCU "Software\Microsoft\Internet Explorer\PageSetup" "header" ""
  WriteRegStr HKCU "Software\Microsoft\Internet Explorer\PageSetup" "margin_bottom" "0"
  WriteRegStr HKCU "Software\Microsoft\Internet Explorer\PageSetup" "margin_left" "0"
  WriteRegStr HKCU "Software\Microsoft\Internet Explorer\PageSetup" "margin_right" "0"
  WriteRegStr HKCU "Software\Microsoft\Internet Explorer\PageSetup" "margin_top" "0"
  WriteRegStr HKCU "Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" "$INSTDIR\恒馨收银系统.exe" "RUNASADMIN"  /**以管理员身份运行**/
  /*******添加用户注册表--end*******/
SectionEnd

/******************************
 *  以下是安装程序的卸载部分  *
 ******************************/

Section Uninstall
  !insertmacro MUI_STARTMENU_GETFOLDER "Application" $ICONS_GROUP
  Delete "$INSTDIR\uninst.exe"

  Delete "$SMPROGRAMS\$ICONS_GROUP\Uninstall.lnk"
  
  Delete "$SMPROGRAMS\$ICONS_GROUP\卸载.lnk"
  Delete "$SMPROGRAMS\$ICONS_GROUP\${PRODUCT_NAME}.lnk"
  Delete "$DESKTOP\${PRODUCT_NAME}.lnk"

  RMDir "$SMPROGRAMS\$ICONS_GROUP"

  RMDir /r "$INSTDIR\*.*"

  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  SetAutoClose true
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "您确实要完全移除 $(^Name) ，及其所有的组件？" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功地从您的计算机移除。"
FunctionEnd
