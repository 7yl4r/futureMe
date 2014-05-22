'''
Accepts any arguments, does no actual image/model processing, and returns a dict of features all set to default values.
'''

def getFeatures(**kwargs):
	return dict(
            age=30,
			gender=0.9,
            race='caucasian',
            muscle='.5',
            weight='.5',
            hieght='.5',
            proportions='.5'
    )
			