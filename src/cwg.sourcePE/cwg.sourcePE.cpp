#include <iostream>
#include <string>

using namespace std;

int main()
{
	cout << "Owned by CWG" << endl;

	string entryPoint = "CWGENTRYPT";

	string buffer = "";

	for (auto x = 0; x < 10; x++)
	{
		buffer += entryPoint;
	}

	int count;

	cin >> count;

	return 0;
}