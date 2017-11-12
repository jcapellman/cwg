#include <iostream>
#include <curl/curl.h>

using namespace std;

static size_t WriteCallback(void *contents, size_t size, size_t nmemb, void *userp)
{
	((std::string*)userp)->append((char*)contents, size * nmemb);
	return size * nmemb;
}

int main()
{
	cout << "Owned by CWG" << endl;

	CURL *curl;
	CURLcode res;
	curl = curl_easy_init();
	std::string readBuffer;

	curl_easy_setopt(curl, CURLOPT_HTTPGET, 1);
	curl_easy_setopt(curl, CURLOPT_URL, "http://cwg.io/");
	curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, WriteCallback);
	curl_easy_setopt(curl, CURLOPT_WRITEDATA, &readBuffer);

	res = curl_easy_perform(curl);

	curl_easy_cleanup(curl);

	cout << readBuffer.c_str() << endl;

    return 0;
}