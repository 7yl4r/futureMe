'''
Uses makehuman.org to create a model
'''

__author__ = 'Tylar Murray'

from subprocess import call

def makeModel(features, makehumanPath, loc):
    '''
    Converts features to model.
    Returns model location if success, else returns false.
    '''
    cmdstr = _dict2cmdstr(features)
    call(makehumanPath + cmdstr + " -o "+loc)


# PRIVATE FUNCTIONS

def _dict2cmdstr(dic):
    '''
    Converts cmd-line options dict into cmd-line string for use with makehuman.
	Mapping is based on [this documentation](http://www.makehuman.org/doc/node/maketarget_standalone.html),
	last updated 2014-05-20.
    '''
    cmdstr = ''

    # TODO: map dict keys to mh cmd args

# === relevant parts from makehuman -h : ===
#     Macro properties:
#   Optional macro properties to set on human
#   --age AGE             Human age, in years
#   --gender GENDER       Human gender (0.0: female, 1.0: male)
#   --male                Produces a male character (overrides the gender
#                         argument)
#   --female              Produces a female character (overrides the gender
#                         argument)
#   --race RACE           One of [caucasian, asian, african]


# === Modifiers loading: ===
#   -m modifierName value, --modifier modifierName value
#                         Specify a modifier and its value to apply to the human
    # === available modifiers (from --listmodifiers): ===
      #   Available modifier names:
      # head/head-age-less|more
      # head/head-angle-in|out
      # head/head-skinny|fat
      # head/head-oval
      # head/head-round
      # head/head-rectangular
      # head/head-square
      # head/head-triangular
      # head/head-invertedtriangular
      # head/head-diamond
      # head/head-scale-depth-less|more
      # head/head-scale-horiz-less|more
      # head/head-scale-vert-more|less
      # head/head-trans-in|out
      # head/head-trans-down|up
      # head/head-trans-forward|backward
      # forehead/forehead-trans-depth-forward|backward
      # forehead/forehead-scale-vert-less|more
      # forehead/forehead-nubian-less|more
      # forehead/forehead-temple-in|out
      # eyebrows/eyebrows-trans-depth-less|more
      # eyebrows/eyebrows-angle-up|down
      # eyebrows/eyebrows-trans-vert-less|more
      # neck/neck-scale-depth-less|more
      # neck/neck-scale-horiz-less|more
      # neck/neck-scale-vert-more|less
      # neck/neck-trans-horiz-in|out
      # neck/neck-trans-vert-down|up
      # neck/neck-trans-depth-forward|backward
      # eyes/r-eye-height1-min|max
      # eyes/r-eye-height2-min|max
      # eyes/r-eye-height3-min|max
      # eyes/r-eye-push1-in|out
      # eyes/r-eye-push2-in|out
      # eyes/r-eye-move-in|out
      # eyes/r-eye-move-up|down
      # eyes/r-eye-size-small|big
      # eyes/r-eye-corner1-up|down
      # eyes/r-eye-corner2-up|down
      # eyes/l-eye-height1-min|max
      # eyes/l-eye-height2-min|max
      # eyes/l-eye-height3-min|max
      # eyes/l-eye-push1-in|out
      # eyes/l-eye-push2-in|out
      # eyes/l-eye-move-in|out
      # eyes/l-eye-move-up|down
      # eyes/l-eye-size-small|big
      # eyes/l-eye-corner1-up|down
      # eyes/l-eye-corner2-up|down
      # nose/nose-trans-vert-up|down
      # nose/nose-trans-depth-forward|backward
      # nose/nose-trans-horiz-in|out
      # nose/nose-scale-vert-incr|decr
      # nose/nose-scale-horiz-incr|decr
      # nose/nose-scale-depth-incr|decr
      # nose/nose-nostril-width-min|max
      # nose/nose-point-width-less|more
      # nose/nose-height-min|max
      # nose/nose-width1-min|max
      # nose/nose-width2-min|max
      # nose/nose-width3-min|max
      # nose/nose-compression-compress|uncompress
      # nose/nose-curve-convex|concave
      # nose/nose-greek-moregreek|lessgreek
      # nose/nose-hump-morehump|lesshump
      # nose/nose-volume-potato|point
      # nose/nose-nostrils-angle-up|down
      # nose/nose-point-up|down
      # nose/nose-septumangle-decr|incr
      # nose/nose-flaring-decr|incr
      # mouth/mouth-scale-horiz-incr|decr
      # mouth/mouth-scale-vert-incr|decr
      # mouth/mouth-scale-depth-incr|decr
      # mouth/mouth-trans-in|out
      # mouth/mouth-trans-up|down
      # mouth/mouth-trans-forward|backward
      # mouth/mouth-lowerlip-height-min|max
      # mouth/mouth-lowerlip-width-min|max
      # mouth/mouth-upperlip-height-min|max
      # mouth/mouth-upperlip-width-min|max
      # mouth/mouth-cupidsbow-width-min|max
      # mouth/mouth-lowerlip-ext-up|down
      # mouth/mouth-angles-up|down
      # mouth/mouth-lowerlip-middle-up|down
      # mouth/mouth-lowerlip-volume-deflate|inflate
      # mouth/mouth-philtrum-volume-increase|decrease
      # mouth/mouth-upperlip-volume-deflate|inflate
      # mouth/mouth-upperlip-ext-up|down
      # mouth/mouth-upperlip-middle-up|down
      # mouth/mouth-cupidsbow-decr|incr
      # ears/r-ear-trans-depth-backward|forward
      # ears/r-ear-size-big|small
      # ears/r-ear-trans-vert-down|up
      # ears/r-ear-height-min|max
      # ears/r-ear-lobe-min|max
      # ears/r-ear-shape1-pointed|triangle
      # ears/r-ear-rot-backward|forward
      # ears/r-ear-shape2-square|round
      # ears/r-ear-width-max|min
      # ears/r-ear-wing-out|in
      # ears/r-ear-flap-out|in
      # ears/l-ear-trans-depth-backward|forward
      # ears/l-ear-size-big|small
      # ears/l-ear-trans-vert-down|up
      # ears/l-ear-height-min|max
      # ears/l-ear-lobe-min|max
      # ears/l-ear-shape1-pointed|triangle
      # ears/l-ear-rot-backward|forward
      # ears/l-ear-shape2-square|round
      # ears/l-ear-width-max|min
      # ears/l-ear-wing-out|in
      # ears/l-ear-flap-out|in
      # chin/chin-prominent-less|more
      # chin/chin-width-min|max
      # chin/chin-height-min|max
      # chin/chin-bones-in|out
      # chin/chin-prognathism-less|more
      # cheek/l-cheek-volume-deflate|inflate
      # cheek/l-cheek-bones-in|out
      # cheek/l-cheek-inner-deflate|inflate
      # cheek/l-cheek-trans-vert-down|up
      # cheek/r-cheek-volume-deflate|inflate
      # cheek/r-cheek-bones-in|out
      # cheek/r-cheek-inner-deflate|inflate
      # cheek/r-cheek-trans-vert-down|up
      # torso/torso-scale-depth-decr|incr
      # torso/torso-scale-horiz-decr|incr
      # torso/torso-scale-vert-decr|incr
      # torso/torso-trans-horiz-in|out
      # torso/torso-trans-vert-down|up
      # torso/torso-trans-depth-forward|backward
      # torso/torso-vshape-less|more
      # hip/hip-scale-depth-decr|incr
      # hip/hip-scale-horiz-decr|incr
      # hip/hip-scale-vert-decr|incr
      # hip/hip-trans-in|out
      # hip/hip-trans-down|up
      # hip/hip-trans-forward|backward
      # stomach/stomach-tone-decr|incr
      # stomach/stomach-pregnant-decr|incr
      # buttocks/buttocks-volume-decr|incr
      # pelvis/pelvis-tone-decr|incr
      # pelvis/bulge-decr|incr
      # armslegs/r-hand-fingers-diameter-decr|incr
      # armslegs/r-hand-fingers-length-decr|incr
      # armslegs/r-hand-scale-decr|incr
      # armslegs/r-hand-trans-in|out
      # armslegs/l-hand-fingers-diameter-decr|incr
      # armslegs/l-hand-fingers-length-decr|incr
      # armslegs/l-hand-scale-decr|incr
      # armslegs/l-hand-trans-in|out
      # armslegs/r-foot-scale-decr|incr
      # armslegs/r-foot-trans-in|out
      # armslegs/r-foot-trans-forward|backward
      # armslegs/l-foot-scale-decr|incr
      # armslegs/l-foot-trans-in|out
      # armslegs/l-foot-trans-forward|backward
      # armslegs/r-lowerarm-scale-depth-decr|incr
      # armslegs/r-lowerarm-scale-horiz-decr|incr
      # armslegs/r-lowerarm-scale-vert-decr|incr
      # armslegs/r-lowerarm-skinny|fat
      # armslegs/r-upperarm-scale-depth-decr|incr
      # armslegs/r-upperarm-scale-horiz-decr|incr
      # armslegs/r-upperarm-scale-vert-decr|incr
      # armslegs/r-upperarm-skinny|fat
      # armslegs/l-lowerarm-scale-depth-decr|incr
      # armslegs/l-lowerarm-scale-horiz-decr|incr
      # armslegs/l-lowerarm-scale-vert-decr|incr
      # armslegs/l-lowerarm-skinny|fat
      # armslegs/l-upperarm-scale-depth-decr|incr
      # armslegs/l-upperarm-scale-horiz-decr|incr
      # armslegs/l-upperarm-scale-vert-decr|incr
      # armslegs/l-upperarm-skinny|fat
      # armslegs/r-leg-genu-varun|valgus
      # armslegs/r-lowerleg-scale-depth-decr|incr
      # armslegs/r-lowerleg-scale-horiz-decr|incr
      # armslegs/r-lowerleg-skinny|fat
      # armslegs/r-upperleg-scale-depth-decr|incr
      # armslegs/r-upperleg-scale-horiz-decr|incr
      # armslegs/r-upperleg-scale-vert-decr|incr
      # armslegs/r-upperleg-skinny|fat
      # armslegs/l-leg-genu-varun|valgus
      # armslegs/l-lowerleg-scale-depth-decr|incr
      # armslegs/l-lowerleg-scale-horiz-decr|incr
      # armslegs/l-lowerleg-skinny|fat
      # armslegs/l-upperleg-scale-depth-decr|incr
      # armslegs/l-upperleg-scale-horiz-decr|incr
      # armslegs/l-upperleg-scale-vert-decr|incr
      # armslegs/l-upperleg-skinny|fat
      # breast/BreastSize
      # breast/BreastFirmness
      # breast/breast-trans-vert-down|up
      # breast/breast-dist-min|max
      # breast/breast-point-min|max
      # breast/breast-volume-vert-up|down
      # genitals/penis-length-min|max
      # genitals/penis-circ-min|max
      # genitals/penis-testicles-min|max
      # macrodetails/Gender	Gender of the human (min is female, max is male).
      # macrodetails/Age	Age of the human (range from 1 year to 90 years old, with center position 25 years).
      # macrodetails/African	African ethnicity of the human (the three ethnicity values are normalized so they sum to 100%).
      # macrodetails/Asian	Asian ethnicity of the human (the three ethnicity values are normalized so they sum to 100%).
      # macrodetails/Caucasian	Caucasian ethnicity of the human (the three ethnicity values are normalized so they sum to 100%).
      # macrodetails-universal/Muscle	Amount of muscle mass.
      # macrodetails-universal/Weight	The weight of the human.
      # macrodetails-height/Height	The height/length of the human.
      # macrodetails-proportions/BodyProportions	Proportions of the human features, often subjectively referred to as qualities of beauty (min is unusual, center position is average and max is idealistic proportions).

#   --material materialFile
#                         Specify a skin material to apply to the human


# === additional (maybe useful) options: ===
# -p proxyType proxyFile, --proxy proxyType proxyFile
#                       Load a proxy of a specific type
#     --rig rigType         Setup a rig. (default: none)
#     --proxymaterial proxyType materialFile
#                       Specify a material to apply to the proxy
    return cmdstr

