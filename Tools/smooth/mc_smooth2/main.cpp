#include "mesh.h"

#include <string>
using std::string;


int main(int argc, char **argv)
{
	if(argc != 2)
	{
		cout << "Example usage: " << argv[0] << " filename.stl" << endl;
		return 1;
	}

	indexed_mesh mesh;

	if(false == mesh.load_from_binary_stereo_lithography_file(argv[1]))
	{
		cout << "Error: Could not properly read file " << argv[1] << endl;
		return 2;
	}

	mesh.fix_cracks();

	float lambda = 0.5f;
	float mu = -0.53f;
	
	mesh.taubin_smooth(lambda, mu, 10);

	string out_file_name = argv[1];
	out_file_name = "smoothed_" + out_file_name;

	if(false == mesh.save_to_binary_stereo_lithography_file(out_file_name.c_str()))
	{
		cout << "Error: Could not properly write file " << out_file_name << endl;
		return 2;
	}

	return 0;
}

