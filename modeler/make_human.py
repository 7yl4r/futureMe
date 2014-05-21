'''
Uses makehuman.org to create a model
'''

__author__ = 'Tylar Murray'


def makeModel(features, loc='./body.mhx'):
    '''
    Converts features to model.
    Returns model location if success, else returns false.
    '''
    cmdstr = _dict2cmdstr(features)
    # call([MH + cmdstr + "--o "+MH_MODEL])


# PRIVATE FUNCTIONS

def _dict2cmdstr(dic):
    '''
    Converts cmd-line options dict into cmd-line string for use with makehuman.
	Mapping is based on [this documentation](http://www.makehuman.org/doc/node/maketarget_standalone.html),
	last updated 2014-05-20.
    '''
    cmdstr = ''

    # TODO: map dict keys to mh cmd args

    return cmdstr

