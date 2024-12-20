import '../styles/footer.css'
export default function Footer() {
    return(
        <>
            <footer className="main-footer">
                <div className="links-container">
                    <div className="links-wrapper">
                        <a className="footer-link" href="/privacy-settings?lang=en" itemProp="url">Privacy Settings</a>
                        <span className="footer-separator"> / </span>
                        <a className="footer-link" href="/communication-rules?lang=en" itemProp="url">Communication Rules</a>
                        <span className="footer-separator"> / </span>
                        <a className="footer-link" href="/cookie-policy?lang=en" itemProp="url">Cookies Policy</a>
                        <span className="footer-separator"> / </span>
                        <a className="footer-link" href="/terms?lang=en" itemProp="url">Terms</a>
                    </div>
                </div>
                <section>
                    <div className="copyright-container">© 2024 Tinder For Pets, All Rights Reserved.</div>
                </section>
            </footer>
        </>
    );
}