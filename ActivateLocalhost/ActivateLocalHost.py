from http.server import HTTPServer, SimpleHTTPRequestHandler
import socket
import ssl

if __name__ == '__main__':
    ## Get free port on localhost
    #sock = socket.socket()
    #sock.bind(('', 0))
    #port = sock.getsockname()[1]

    # Set port
    port = 8888
    input_port = input('Specify port or press enter (port 8888 by default): ')
    try:
        port = int(input_port)
    except ValueError:
        pass  # it was a string, not an int.
    
    # Serve local files
    class CORSRequestHandler (SimpleHTTPRequestHandler):
        def end_headers (self):
            self.send_header('Access-Control-Allow-Origin', '*')
            SimpleHTTPRequestHandler.end_headers(self) 
            

    try:
        httpd = HTTPServer(('localhost', port), CORSRequestHandler)
        httpd.socket = ssl.wrap_socket(httpd.socket, certfile='certfile.pem', server_side=True)
        print ("")
        print ("Serving at https://localhost:" + str(port))
        httpd.serve_forever()
    except:
        print("Error creating port...")
