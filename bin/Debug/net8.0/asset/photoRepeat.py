import os
from PIL import Image

def save_frames_from_gif(gif_path, output_folder):
    with Image.open(gif_path) as img:
        for frame_number in range(img.n_frames):
            img.seek(frame_number)
            frame = img.convert('RGBA')
            frame_path = os.path.join(output_folder, f'player_run_right_{frame_number}.png')
            frame.save(frame_path)
            print(f'Saved {frame_path}')

def flip_and_rename_images(input_folder, output_folder):
    for file_name in os.listdir(input_folder):
        if file_name.endswith('.png') and 'player_run_right_' in file_name:
            img_path = os.path.join(input_folder, file_name)
            img = Image.open(img_path)
            flipped_img = img.transpose(Image.FLIP_LEFT_RIGHT)
            frame_number = file_name.split('_')[-1].split('.')[0]
            flipped_img_path = os.path.join(output_folder, f'player_run_left_{frame_number}.png')
            flipped_img.save(flipped_img_path)
            print(f'Saved {flipped_img_path}')

# Set the paths
gif_path = r'D:\some stuff\college\COS20007 Object Oriented Programming\Cave-dweller\bin\Debug\net8.0\asset\player_run.gif'
output_folder = r'D:\some stuff\college\COS20007 Object Oriented Programming\Cave-dweller\bin\Debug\net8.0\asset\player_run'

# Create the output folder if it does not exist
os.makedirs(output_folder, exist_ok=True)

# Save frames from GIF and rename
save_frames_from_gif(gif_path, output_folder)

# Flip images and rename
flip_and_rename_images(output_folder, output_folder)
