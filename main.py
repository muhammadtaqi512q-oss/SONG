import os
import sys
import webview


def get_resource_path(relative_path):
  try:
    base_path = sys._MEIPASS
  except Exception:
    base_path = os.path.abspath('.')
  return os.path.join(base_path, relative_path)


def main():
  home_file = get_resource_path('home.html')
  local_url = f'file:///{home_file.replace(os.sep, "/")}'

  # Browser Window Creation
  window = webview.create_window(
      title='TAQI Browser - Created by Muhammad Taqi',
      url=local_url,
      width=1200,
      height=800,
      min_size=(900, 600),
      resizable=True,
      confirm_close=False,
  )

  webview.start(debug=False)


if __name__ == '__main__':
  main()
