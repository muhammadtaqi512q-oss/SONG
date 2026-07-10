import random
from flask import Flask, request, jsonify, render_template

app = Flask(__name__)

# Data Structure for Music Links
MUSIC_DATA = {
    "english": {
        "sad": "https://share.gemini.google/vrzHOYScsATb",
        "happy": "https://share.gemini.google/rRMtZComrLFC",
        "funny": "https://share.gemini.google/3kSU1sqmTK2u",
        "default": "https://share.gemini.google/rRMtZComrLFC"
    },
    "urdu": {
        "sad": "https://share.gemini.google/3OSEMGyL8De4",
        "happy": "https://share.gemini.google/NWmstTjVom2k",
        "funny": "https://share.gemini.google/mFP6za5e7XGt",
        "default": "https://share.gemini.google/NWmstTjVom2k"
    }
}

ELSE_LINKS = [
    "https://share.gemini.google/EO0IslZOc11i",
    "https://share.gemini.google/xEx6KhaNZjnp",
    "https://share.gemini.google/b5eJEf30z2D3",
    "https://share.gemini.google/gKPyKAH8odPH",
    "https://share.gemini.google/iKo1z0WxmGCt",
    "https://share.gemini.google/4KTOkTygQxxL",
    "https://share.gemini.google/oAREwuQaVq1v",
    "https://share.gemini.google/bKAz9o66h1mZ",
    "https://share.gemini.google/knSi0DPxXjFZ",
    "https://share.gemini.google/BhLAaGMvqgEq",
    "https://share.gemini.google/RF6mAuYWPeX7",
    "https://share.gemini.google/JVUlIpd25A0M",
    "https://share.gemini.google/pyTB8aaiL4qc"
]

@app.route('/')
def home():
    return render_template('index.html')

@app.route('/mt/king/api/', methods=['POST', 'GET'])
def get_music_api():
    if request.method == 'POST':
        data = request.get_json(silent=True) or {}
        chat_text = data.get('chat', '').lower()
    else:
        chat_text = request.args.get('chat', '').lower()

    if not chat_text:
        return jsonify({
            "status": "success",
            "music_url": random.choice(ELSE_LINKS),
            "message": "No input provided, playing random fallback."
        })

    has_english = 'english' in chat_text
    has_urdu = 'urdu' in chat_text
    
    detected_mood = None
    for mood in ['sad', 'happy', 'funny']:
        if mood in chat_text:
            detected_mood = mood
            break

    if (has_english and has_urdu) or (not has_english and not has_urdu):
        if detected_mood:
            lang = random.choice(['english', 'urdu'])
            music_url = MUSIC_DATA[lang][detected_mood]
        else:
            music_url = random.choice(ELSE_LINKS)

    elif has_english:
        if detected_mood:
            music_url = MUSIC_DATA['english'][detected_mood]
        else:
            music_url = MUSIC_DATA['english']['default']

    elif has_urdu:
        if detected_mood:
            music_url = MUSIC_DATA['urdu'][detected_mood]
        else:
            music_url = MUSIC_DATA['urdu']['default']
            
    else:
        music_url = random.choice(ELSE_LINKS)

    return jsonify({
        "status": "success",
        "chat_received": chat_text,
        "music_url": music_url
    })

if __name__ == '__main__':
    # GitHub Codespace ke liye host="0.0.0.0" hona zaroori ha
    app.run(host="0.0.0.0", port=5000, debug=True)
