'''
This is the main script which runs all subscripts.
'''

from os import listdir, getcwd
from sys import argv
import imp

# === global consts ===
# path to makehuman .exe or .py main on the system
#MH    = 'python C:\Users\\twm4061\Documents\duststorm01-makehuman-commandline-d0aef702b167\makehuman\makehuman.py'
MH = 'python C:\Users\\7yl4r\Documents\makehuman-commandline\makehuman\makehuman.py'

# path to blender on this system
BLEND = 'blender'

# === global vars ===
mh_options = {}  # dict of options to pass to makehuman
scene_script = 'basic'  # path to the script to use in blender



# get images, head model
IMAGES = [getcwd()+'/data/img/'+str(f) for f in listdir('./data/img/')]
HEAD_SCAN= getcwd()+'/data/3dscan/mesh.obj'  # path to the head scan model

# get base user data for model
if len(argv) > 1:  # get user data from given file
    print 'loading user data from ', argv[1]
    USER_DATA = imp.load_source('module.name', argv[1])
else:
    print 'no user data file given'
    raise NotImplementedError('must specify user data file')
    
# process images to get features (user features are selected user data (plus extracted features) used to generate the model)
import image_processor.mock  # TODO: use real image processor
user_features = image_processor.mock.getFeatures(images=IMAGES, headScan=HEAD_SCAN, userData=USER_DATA)

# create model
from modeler.make_human import makeModel
CURRENT_BODY = getcwd()+'/data/currentMe.mhx'  # path to the body model
makeModel(user_features, MH, CURRENT_BODY)


# TODO: compute changes to model conditioned on user data
# (converts features+data+time into future features)
# import predictor.mock.predictFeatures as predictFeatures
future_features = user_features # predictFeatures(user_features, USER_DATA)


# TODO: modify body model
FUTURE_BODY = getcwd()+'/data/futureMe.mhx'  # path to the body model
makeModel(future_features, MH, FUTURE_BODY)


# TODO: make the scene
# import viewer.blender.makeScene as makeScene  # NOTE: blender is a modeler & viewer. shouldn't the "model" from modeler be head+body?
# makeScene(CURRENT_BODY, FUTURE_BODY, ...)
# this should do something like... call([BLEND, '-P', scene_script]

# TODO: view (or send to viewer) the rendering
