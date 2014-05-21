'''
This is the main script which runs all subscripts.
'''

from subprocess import call

# global consts
MH    = './makehuman.py'  # path to makehuman .exe or .py main on the system
BLEND = 'blender'  # path to blender on this system

# global vars
mh_options = {}  # dict of options to pass to makehuman
scene_script = 'basic'  # path to the script to use in blender

# get images, head model, user data
IMAGES = ['./IMG/'+str(i)+'.png' for i in range(0,3)]
HEAD_SCAN= './head.obj'  # path to the head scan model

# process images to get features
import image_processor.mock  # TODO: use real image processor
user_features = image_processor.mock.getFeatures(images=IMAGES, headScan=HEAD_SCAN)

# create model
import modeler.make_human.makeModel as makeModel
CURRENT_BODY = './currentMe.mhx'  # path to the body model
makeModel(user_features, loc=CURRENT_BODY)


# TODO: compute changes to model conditioned on user data
# (converts features+data+time into future features)
# import predictor.mock.predictFeatures as predictFeatures
future_features = user_features # predictFeatures(user_features, data)


# TODO: modify body model
FUTURE_BODY = './futureMe.mhx'  # path to the body model
makeModel(future_features, loc=FUTURE_BODY)


# TODO: make the scene
# import viewer.blender.makeScene as makeScene  # NOTE: blender is a modeler & viewer. shouldn't the "model" from modeler be head+body?
# makeScene(CURRENT_BODY, FUTURE_BODY, ...)
# this should do something like... call([BLEND, '-P', scene_script]

# TODO: view (or send to viewer) the rendering
