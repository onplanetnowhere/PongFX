Just a basic list of what all the settings are:

Assets > SilVR > WaterShader > Water_Surface_M >

	General Camera Note: 	The cameras will update their view aspect ratio
	according to the aspect ratio of their Render texture, but they may not
	update it until you change 'size' do a different value and back.

	Camera_In - 		The render texture that the camera 'Camera_In' Renders to.
				The x:y ratio of the textures resolution should be the same
				as the x:z ratio of the WaterSurface Plane 

	ColMask -		A Render texture for generating collision masks. Can be any
				resolution but I reccomend a similar aspect ratio to the x:z
				ratio of the plane's scale.

	Render_Plane - 		A Render texture for calculating the ripples and normal maps.
				This MUST be in an aspect ration of 2x:z of the plane, or 2x:y
				of the render texure Camera_In.

	Render_plane.mat -	The material that goes on the 'Render_Plane' plane. It is the
				method of calculating the ripple effect. It has several settings

		Render Plane -	The Render_Plane render texture

		Camera In -	The Camera_In render texture

		Wave Divisor - 	1/Wave Divisor is the distance the algorithm checks for nearby
				waves. Use this to adjust the crispness of the wave. I usually
				use roughly 1.5 times the resolution, but actual resolution
				works just fine. Setting it too far from the actual resolution
				will cause some graphical glitching, however.

		Damping -	The amount of Damping in the wave algorithm. .99 works best

		ObjectWave Height - The height of a wave caused by any interfering object

		Smoothing Factor - A smoothing factor that works best set to 0 for water.
				may be useful for other effects, but I don't usually use it.

		Aspect Ratio -	The x:z aspect ratio for the water surface plane.

		Border Size -	The border size in pixels IF the ripple divisor is set to the
				actual resolution. This setting should only really be used if
				one of the edges bleeds over onto the opposite edge. Default
				is 1.5, which is good for most resolutions.

		Water Mask Map - A Masking image for water collision simulation. Any section
				that is not black on the mask will be treated as if it were a wall
				or border.

		Water Mask Weight - The amount that the water mask actually influences collison.
				Set this to either 0, or 1.

	Unlit_Debug - 		A material used to view mainly the collision mask.

	Water Surface.mat -	The material specifically for the water surface. It is a modified
				standard surface shader with cubemap support found in the unity
				shader documentation.	

		Render Plane - 	Render plane Render texture

		Cubemap - 	Cubemap being used for reflection. The texture must be in a cube-
				map format. It is best to use an equirectangular skybox set to 
				'Cube' instead of '2D', with a cylindrical (latitude longitude)
				mapping setup. If the cubemap/skybox you wish to use is in a 
				6-sided cube setup I recommend trying to find a program online or
				online converter to change it to equirectangular.

		True Normal -	Just set this to 1

		Ripple Divisor - The distance used to convert the wave map to a normal map. I like
				to use one half of the actual resolution.

		Alpha -		The transparency of the water after applying the alpha weight mask

		Transparency Mask - The mask used to control only portions of the water's alpha.
				White means completely visible.

		Alpha Mask Weight - How much the Transparency mask affects the water surface.

	Water_Rig.prefab - the prefab for the water rig.

Assets > SilVR > WaterShader > Water_Surface_M > Cubemaps

	00_Panosphere -		A Standard vrchat panosphere shader. Takes in equirectangular textures
				set to '2D' instead of 'cube'. This is the material that will go onto
				00_uv_Cube and has an offset in order to align the reflection of the
				water with the simulated skybox.

	00_uv_Cube -		A large cube with flipped normals for using as a skybox. The advantages
				of using this method of skybox simulation is that the material can be
				changed with an animation so that multiple skyboxes can be used and
				switched through at the users preference.

Legacy shaders are included as well should you wish to use a regular skybox reflection, but it is not
recommended as the reflections will either not be lined up properly (and cannot be lined up properly),
or it will be twice as performance heavy as this particular version. If you wish to squeeze more
performance out of the shader, some render textures can be changed to lighter formats, but the code may
need to be modified for the shader to work properly.