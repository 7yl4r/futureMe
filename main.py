'''
This is the main script which runs all subscripts.
'''

from subprocess import call

MH    = './makehuman.py'  # path to makehuman .exe or .py main on the system
BLEND = 'blender'  # path to blender on this system

mh_options = dict()  # dict of options to pass to makehuman
scene_script = 'basic'  # path to the script to use in blender

def dict2cmdstr(dic):
    '''
    converts cmd-line options dict into cmd-line string for use with makehuman
    '''
    cmdstr = ''
    
    # TODO: map dict keys to mh cmd args 
    
    return cmdstr

# TODO: get images, model, user data

# TODO: create makehuman model
# call([MH + cmdstr])

# TODO: open blender scene-making script
# call([BLEND, '-P', scene_script]

# TODO: view (or send to viewer) the rendering made by makehuman
