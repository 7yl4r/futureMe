'''
Accepts any arguments, does no actual image/model processing, and returns a dict of features all set to default values. 
If userData is given in kwargs, the value included are passed through unaltered and missing ones are set to default.
'''

USER_DATA_KEY = 'userData'

def _attempt(kwargs, key, default):
    '''
    like if-else ternary ( ? : ) but for try-except
    '''
    try:
        val = getattr(kwargs[USER_DATA_KEY],key)
    except Exception:
        val = default
    return val
        
def getFeatures(**kwargs):
	return dict(
    #        age        = _attempt(kwargs,'age',30),
	#		gender     = _attempt(kwargs,'gender', .9),
      #      race       = _attempt(kwargs,'race','caucasian'),
      #      muscle     = _attempt(kwargs,'muscle',.5),     #NOTE: all values btwn 1000 and .0001 give same, super-muscular resut...
            weight     = _attempt(kwargs,'weight',1000),
    #        hieght     = _attempt(kwargs,'height',.3),
            proportions= _attempt(kwargs,'proportions',.5)
    )
			