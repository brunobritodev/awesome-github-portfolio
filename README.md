# 🧠 Awesome GitHub Portfolio

This open-source project automatically generates a beautiful and professional portfolio page based on your public GitHub activity.

It's perfect for developers who want to showcase their real-world experience — not just tell, but show.

## ✨ Features

- 🔍 Extracts and displays your GitHub contributions
- 📊 Ranks repositories by popularity and activity
- 🎨 Clean and modern UI with responsive design
- ⚙️ Easy to deploy with Docker
- 🧠 Optional GPT-4o integration for enhanced descriptions *(for followers)*

## 🛠 Technologies Used

- C#
- HTML/CSS
- JavaScript
- GitHub GraphQL API
- Docker

## 🚀 How to Use

To generate your portfolio, simply go to:

🔗 [portfolio website](https://portfolio.avera.com.br)

Your GitHub username and a star on this repository are required to unlock the full experience.

If you’d like to contribute or develop locally, follow the steps below:

1. **Clone this repository**

```
git clone https://github.com/brunobritodev/awesome-github-portfolio.git
```

2. **Navigate to the project directory**

```
cd awesome-github-portfolio
```

3. **Build the Docker image**

```
docker build -t awesome-github-portfolio .
```

4. **Run the Docker container**

```
docker run -d -p 8080:8080 awesome-github-portfolio
```

5. **Access the application in your browser**

```
http://localhost:8080
```

## 🌟 Important Notes

- You must star the repository for your portfolio to be generated.
- If you want to customize your portfolio:
  - Download your portfolio data (`resume.json`)
  - Fork this repository
  - Replace the `resume.json` in the root with your custom version
  - Go again to [portfolio website](https://portfolio.avera.com.br)

## 💡 Bonus for Followers

If you follow [@brunobritodev](https://github.com/brunobritodev) on GitHub, you'll unlock the GPT-4o enhanced generation — with smarter summaries and automatic highlights of standout projects.

Also, yes — your star helps prove to my mom that I'm popular 😅

## 🤝 Contributing

Contributions are welcome! Feel free to fork the project, open issues, and submit pull requests to help improve it.

## 📄 License

This project is licensed under the MIT License.
