import pycurl
import certifi
import json
import os
import mimetypes
from StringIO import StringIO

from subprocess import check_output
import re
import glob
import sys

dev = False

if len(sys.argv) > 1:
  dev = True

out = check_output(["git", "describe", "--long"]).replace('-', '.', 1).replace("\r","").replace("\n","")
arr = out.split(".")
var = arr[0:-1]
if len(var) < 3:
  var = var + ['0'] * (3 - len(arr))

if dev:
  vars = var + [arr[-1].split('-')[0]]
  varl = var + [arr[-1][0]+'-'+sys.argv[1]]
else:
  vars = var + [arr[-1].split('-')[0]]
  varl = var + [arr[-1]]

def tagOnly():
  return ".".join(var[0:3])

def tagOnlyWithComma():
  return ".".join(var[0:3])

def longVersion():
  return ".".join(varl)

def longVersionWithComma():
  return ",".join(varl)
  
def shortVersion():
  return ".".join(vars)

def shortVersionWithComma():
  return ",".join(vars)
  


def get_releases(owner, repo):
	buffer = StringIO()
	c = pycurl.Curl()
	c.setopt(pycurl.CAINFO, certifi.where())
	c.setopt(c.URL, "https://api.github.com/repos/{}/{}/releases".format(owner, repo))
	c.setopt(c.WRITEDATA, buffer)
	c.setopt(c.WRITEFUNCTION, buffer.write)
	c.perform()
	c.close()
	body = buffer.getvalue()
	return json.loads(body)

def upload_file(url, filename, token):
	buffer = StringIO()
	c = pycurl.Curl()
	c.setopt(pycurl.CAINFO, certifi.where())
	c.setopt(c.URL, url + "?name={0}".format(os.path.basename(filename)))
	c.setopt(c.HTTPHEADER, [
		"Content-type: application/octet-stream",
		"Authorization: token {}".format(token)])
	c.setopt(c.POST, 1)
	c.setopt(c.UPLOAD, 1)
	c.setopt(c.READFUNCTION, open(filename, 'rb').read)
	filesize = os.path.getsize(filename)
	c.setopt(c.INFILESIZE, filesize)
	c.setopt(c.WRITEDATA, buffer)
	c.setopt(c.WRITEFUNCTION, buffer.write)
	c.perform()
	c.close()
	body = buffer.getvalue()
	return json.loads(body)

apitoken = sys.argv[1]
owner = sys.argv[2]
repo = sys.argv[3]
tag = sys.argv[4]
file = sys.argv[5]

releases = get_releases(owner, repo)
for release in releases:
	if release["tag_name"] == tag:
		upload_url = release["assets_url"].replace("api.github", "uploads.github")
		print(upload_url)
		s = upload_file(upload_url, file, apitoken)
		print(s)
