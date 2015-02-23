@ECHO OFF
SET VLC_EXE="c:\Program Files (x86)\VideoLAN\vlc\vlc.exe"

SET file_name=data3

SET file_path=c:\doc\Projects\C#\GoogleSpeech\GoogleSpeech\work_dir

SET SRC_File=%file_path%\%file_name%.wav

SET DST_File=%file_path%\%file_name%.flac

SET transcode_options=vcodec=none,acodec=flac,ab=16,channels=1,samplerate=8000

::—- HIDE the VLC interface & WORK !!!
%VLC_EXE% –file-caching=300 "c:\doc\Projects\C#\GoogleSpeech\GoogleSpeech\work_dir\data3.wav" –sout #transcode{%transcode_options%}:file{dst='%DST_File%'} -I dummy 
rem vlc://quit