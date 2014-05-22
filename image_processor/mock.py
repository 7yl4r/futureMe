'''
Accepts any arguments, does no actual image/model processing, and returns a dict of features all set to default values.
'''

def getFeatures(**kwargs):
	return dict(
            fitness=0.5,
			height=2
    )
			