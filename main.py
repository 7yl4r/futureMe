'''
This is the main script which runs all subscripts.
'''

from os import listdir, getcwd

# global consts
MH    = 'python C:\Users\twm4061\Documents\duststorm01-makehuman-commandline-d0aef702b167\makehuman\makehuman.py'  # path to makehuman .exe or .py main on the system
BLEND = 'blender'  # path to blender on this system

# global vars
mh_options = {}  # dict of options to pass to makehuman
scene_script = 'basic'  # path to the script to use in blender

# get images, head model, user data
IMAGES = [getcwd()+'/data/img/'+str(f) for f in listdir('./data/img/')]
HEAD_SCAN= getcwd()+'/data/3dscan/mesh.obj'  # path to the head scan model

# process images to get features
import image_processor.mock  # TODO: use real image processor
user_features = image_processor.mock.getFeatures(images=IMAGES, headScan=HEAD_SCAN)

# create model
from modeler.make_human import makeModel
CURRENT_BODY = getcwd()+'/data/currentMe.mhx'  # path to the body model
makeModel(user_features, MH, CURRENT_BODY)


# TODO: compute changes to model conditioned on user data
# (converts features+data+time into future features)
# import predictor.mock.predictFeatures as predictFeatures
future_features = user_features # predictFeatures(user_features, data)


# TODO: modify body model
FUTURE_BODY = getcwd()+'/data/futureMe.mhx'  # path to the body model
makeModel(future_features, MH, FUTURE_BODY)


# TODO: make the scene
# import viewer.blender.makeScene as makeScene  # NOTE: blender is a modeler & viewer. shouldn't the "model" from modeler be head+body?
# makeScene(CURRENT_BODY, FUTURE_BODY, ...)
# this should do something like... call([BLEND, '-P', scene_script]

# TODO: view (or send to viewer) the rendering
