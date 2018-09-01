# NamesiloAuto

Program starts a new Chrome browser and navigates to Login page. Chrome browser has the Lastpass add-on and logins automatically. Program waits for X seconds to make sure user has been logged in.
Program checks the local folder BID_DATA repeatedly every Y seconds or milliseconds for files with extension .TXT
When a file with extension .TXT is found, program acts as follows :
1) The file's name is the URL and the bid, separated by 3 "_", for instance ZmRmZGZmZmVmBGZ2nnpp27sn07r5s914rq6s835ps2q77143___1.TXT
2) Program saves these details internally and renames the file to ".DONE"
3) Program opens the URL in a new Chrome tab and places the bid

Program Options
X : wait X seconds to make user user has been logged in automatically by Lastpass add-on
Y : check the folder contents every X milliseconds (500 ms = 0.5 sec)
U : URL prefix : default is Namesilo.com/Auctions?auction=     but it changed recently and may change again
M : max bid : Do not place bids that are more than M  (for peace of mind)


Can be run accordingly to instructions from WEB_AUTOMATION folder.
