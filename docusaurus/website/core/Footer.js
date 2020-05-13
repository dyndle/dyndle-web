/**
 * Copyright (c) 2017-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

const React = require("react");

class Footer extends React.Component {
  docUrl(doc, language) {
    const baseUrl = this.props.config.baseUrl;
    const docsUrl = this.props.config.docsUrl;
    const docsPart = `${docsUrl ? `${docsUrl}/` : ""}`;
    const langPart = `${language ? `${language}/` : ""}`;
    return `${baseUrl}${docsPart}${langPart}${doc}`;
  }

  pageUrl(doc, language) {
    const baseUrl = this.props.config.baseUrl;
    return baseUrl + (language ? `${language}/` : "") + doc;
  }

  render() {
    const url = this.props.config.url;
    return (
      <footer className="nav-footer" id="footer">
        <section className="sitemap">
          <a href={this.props.config.baseUrl} className="nav-home">
            {this.props.config.footerIcon && (
              <img
                src={this.props.config.baseUrl + this.props.config.footerIcon}
                alt={this.props.config.title}
                width="66"
                height="58"
              />
            )}
          </a>
          <div>
            <h5>Dyndle</h5>
            <a href={url}>Home</a>
            <a href={this.docUrl("index.html", this.props.language)}>Docs</a>
            <a href="https://trivident.com/">Company behind Dyndle</a>
          </div>
          <div>
            <h5>Community</h5>
            <a href="https://github.com/dyndle">GitHub</a>
            <a
              href="https://tridion.stackexchange.com/questions/tagged/dyndle"
              target="_blank"
              rel="noreferrer noopener"
            >
              Stack Exchange
            </a>
            <a
              href="https://twitter.com/dyndle"
              target="_blank"
              rel="noreferrer noopener"
            >
              Twitter
            </a>
          </div>
          <div>
            <h5>More</h5>
            <a href="http://blog.trivident.com/">Blog</a>
            <a
              className="github-button"
              href="https://github.com/dyndle/dyndle-web"
              data-icon="octicon-star"
              data-count-href="/dyndle/dyndle-web/stargazers"
              data-show-count="true"
              data-count-aria-label="# stargazers on GitHub"
              aria-label="Star this project on GitHub"
            >
              Star
            </a>
            <div className="social">
              <a
                href="https://twitter.com/dyndle"
                className="twitter-follow-button"
              >
                Follow Dyndle
              </a>
            </div>
          </div>
        </section>

        <section className="copyright">{this.props.config.copyright}</section>
      </footer>
    );
  }
}

module.exports = Footer;
