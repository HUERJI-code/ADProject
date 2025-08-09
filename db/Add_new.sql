CREATE DATABASE  IF NOT EXISTS `adproject` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `adproject`;
-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: localhost    Database: adproject
-- ------------------------------------------------------
-- Server version	8.0.39

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `activities`
--

DROP TABLE IF EXISTS `activities`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `activities` (
  `ActivityId` int NOT NULL AUTO_INCREMENT,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Location` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `StartTime` varchar(255) NOT NULL,
  `EndTime` varchar(255) NOT NULL,
  `Status` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedBy` int NOT NULL,
  `url` varchar(255) NOT NULL,
  `number` int NOT NULL,
  PRIMARY KEY (`ActivityId`),
  KEY `IX_Activities_CreatedBy` (`CreatedBy`),
  CONSTRAINT `FK_Activities_Users_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=164 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `activities`
--

LOCK TABLES `activities` WRITE;
/*!40000 ALTER TABLE `activities` DISABLE KEYS */;
INSERT INTO `activities` VALUES (1,'Python Coding Marathon','An intensive Python algorithm coding marathon','Gardens by the Bay, Singapore','2025-09-10 09:00:00','2025-09-10 18:00:00','Approved',2,'http://example.com/events/1',150),(2,'AI Symposium','Deep dive into AI trends and applications','Marina Bay Sands, Singapore','2025-10-05 13:30:00','2025-10-05 17:00:00','Pending',2,'http://example.com/events/2',80),(3,'Bay Area Music Festival','Two-day outdoor music festival featuring multiple bands','Sentosa Island, Singapore','2025-11-20 16:00:00','2025-11-21 22:00:00','Approved',2,'http://example.com/events/3',500),(4,'Creative Writing Workshop','Interactive session to improve creative writing skills','National Library Building, Singapore','2025-08-15 10:00:00','2025-08-15 15:00:00','Cancelled',2,'http://example.com/events/4',30),(5,'Tech Startup Salon','Share startup experiences and incubation processes','one-north, Singapore','2025-09-25 14:00:00','2025-09-25 17:00:00','Approved',2,'http://example.com/events/5',60),(6,'Urban Photography Exhibition','Showcase cityscape works by young photographers','Clarke Quay, Singapore','2025-10-12 09:00:00','2025-10-18 18:00:00','Pending',2,'http://example.com/events/6',120),(7,'River Cleanup Volunteer Drive','Organize river cleanup, recruit eco-volunteers','East Coast Park, Singapore','2025-08-30 08:00:00','2025-08-30 12:00:00','Approved',2,'http://example.com/events/7',45),(8,'Youth Networking Meetup','Monthly youth networking and sharing session','Orchard Road, Singapore','2025-09-01 18:00:00','2025-09-01 20:00:00','Approved',2,'http://example.com/events/8',25),(9,'Morning Run Club','Weekly Sunday morning group run for fitness & networking','Fort Canning Park, Singapore','2025-08-09 06:30:00','2025-08-09 08:00:00','Cancelled',2,'http://example.com/events/9',100),(10,'Blockchain Bootcamp','Hands-on blockchain fundamentals for developers','Marina Barrage, Singapore','2025-10-08 09:30:00','2025-10-10 17:30:00','Pending',2,'http://example.com/events/10',40),(11,'Children\'s Art Contest','Art contest for ages 6–12, theme “My Hometown”','Singapore Art Museum, Singapore','2025-09-18 08:00:00','2025-09-18 12:00:00','Approved',2,'http://example.com/events/11',200),(12,'International Food Fair','World cuisine fair with cultural talks','Suntec City, Singapore','2025-11-05 11:00:00','2025-11-07 20:00:00','Pending',2,'http://example.com/events/12',350),(14,'Python Coding Marathon','Join this intense 24-hour coding marathon where student teams tackle real-world problems provided by local tech companies. With mentor guidance, you\'ll work on projects involving data visualization and machine learning using Python tools. Win Arduino kits and internship interviews! Midnight pizza included to keep you coding.','Marina Bay','2024/10/12 10:00','2024/10/13 10:00','approved',6,'https://example.com/activity/6463',80),(15,'Campus Ping Pong Championship','Showcase your table tennis skills in this inter-department tournament! We\'ll have professional-grade tables and equipment set up in the sports complex. The event features single/double elimination brackets, beginner coaching sessions, and free energy drinks for participants. Winners receive custom paddles and campus bragging rights.','Marina Bay','2024/11/8 14:00','2024/11/8 18:00','approved',2,'https://example.com/activity/9369',32),(16,'Startup Pitch Workshop','Master the art of pitching your startup idea in this intensive workshop. Through interactive exercises, you\'ll learn storytelling techniques, financial modeling basics, and investor psychology. Bring your business concept for live feedback sessions. Successful alumni founders will share their pitching journey from dorm room to boardroom.','Toa Payoh','2024/9/25 16:30','2024/9/25 19:00','approved',6,'https://example.com/activity/7537',45),(17,'Sunrise Meditation Retreat','Begin your day with guided mindfulness meditation in our botanical gardens. Certified instructors will teach breathing techniques and body awareness exercises. We\'ll provide yoga mats and blankets as you experience tranquility amidst nature. Perfect for stress relief during midterms, with herbal tea served afterward.','Woodlands','2024/10/5 6:00','2024/10/5 8:00','approved',6,'https://example.com/activity/7526',9999),(18,'Sustainable Fashion Show','Discover creative upcycling at our sustainable fashion showcase! Student designers transform thrifted clothing into runway-worthy pieces. Learn mending techniques at DIY stations and participate in our clothing swap corner. Ethical fashion brands will offer discounts, and all proceeds support campus recycling initiatives.','Toa Payoh','2024/11/15 19:00','2024/11/15 21:30','approved',6,'https://example.com/activity/6059',120),(19,'International Food Fair','Embark on a global culinary adventure without leaving campus! Student cultural clubs prepare authentic dishes from over 20 countries. Enjoy live cooking demonstrations, recipe trading cards, and cultural performances. Ticket includes 5 tasting portions - vote for your favorite booth to win the Golden Spatula award!','Changi','2024/9/30 17:00','2024/9/30 20:00','approved',6,'https://example.com/activity/9960',300),(20,'VR Game Development Lab','Dive into virtual reality game creation using Unity and Oculus headsets. Learn 3D environment design, motion controls, and interactive storytelling. No prior experience needed - we\'ll guide you from concept to playable prototype. Best student creation gets featured in the campus tech showcase.','Yishun','2024/10/18 13:00','2024/10/18 16:00','approved',9,'https://example.com/activity/2385',25),(21,'Urban Gardening Club','Get your hands dirty at our campus community garden! Learn organic planting techniques for seasonal vegetables and herbs. We\'ll prepare garden beds, plant seedlings, and install irrigation systems. Take home starter kits for windowsill gardens. Monthly harvest parties planned throughout the semester!','Clementi','2024/9/12 15:00','2024/9/12 17:00','approved',2,'https://example.com/activity/9712',40),(22,'Improv Comedy Night','Unleash your creativity at our no-script comedy jam! Professional improvisers teach scene-building games and character development. Volunteers can join on-stage antics in a supportive environment. Features themed improv battles and audience suggestion challenges. Free popcorn and laughter guaranteed!','Bukit Timah','2024/10/25 19:00','2024/10/25 21:00','approved',6,'https://example.com/activity/7456',60),(23,'Resume Revamp Bootcamp','Overhaul your resume with HR professionals from top companies. Learn to highlight student experiences effectively, optimize for ATS systems, and craft compelling achievement statements. Includes 1:1 review sessions, LinkedIn profile tips, and templates for different industries. Bring your current resume for immediate transformation!','Clementi','2024/10/3 14:00','2024/10/3 16:30','approved',9,'https://example.com/activity/1782',35),(24,'Stargazing & Astrophysics Night','Explore the cosmos through high-powered telescopes on the observatory roof! Astronomy professors guide constellation tours and explain celestial phenomena. Participate in meteor-spotting contests and heated debates about space exploration. Hot chocolate and space-themed snacks provided while you ponder the universe.','Changi','2024/11/2 20:00','2024/11/2 23:00','approved',6,'https://example.com/activity/3616',75),(25,'Vintage Video Game Tournament','Relive gaming history with our retro console tournament! Battle on original NES, Sega Genesis, and PlayStation systems. Featured games include Super Smash Bros, Sonic, and Street Fighter. Between matches, explore gaming history exhibits and try indie game demos. Costume contest for best 90s gamer outfit!','Changi','2024/10/29 18:00','2024/10/29 22:00','approved',2,'https://example.com/activity/5045',50),(26,'Podcast Production Masterclass','Launch your podcast from concept to distribution! Learn scriptwriting, recording techniques with professional equipment, and audio editing in Audacity. We\'ll cover interview skills, storytelling structures, and promotion strategies. Participants can book follow-up studio time to record their first episode.','Marina Bay','2024/9/19 15:30','2024/9/19 18:00','approved',6,'https://example.com/activity/5958',30),(27,'Wilderness First Aid Training','Gain certified wilderness first response skills in this hands-on course. Practice wound management, splinting techniques, and emergency scenario responses. Course includes CPR certification and outdoor safety planning. Ideal for hiking club members and outdoor enthusiasts. Certification valid for 2 years.','Marina Bay','2024/11/11 9:00','2024/11/11 16:00','approved',6,'https://example.com/activity/1191',24),(28,'Financial Literacy Workshop','Take control of your finances with practical money management skills. Learn budgeting strategies for student life, debt management principles, and beginner investment concepts. Interactive activities include \'rent vs buy\' simulations and grocery budgeting challenges. Receive personalized financial health assessments.','Orchard','2024/10/7 17:00','2024/10/7 19:00','approved',9,'https://example.com/activity/6672',50),(29,'Digital Art & Animation Lab','Explore digital creativity with industry-standard tools! Learn Photoshop techniques, 2D animation basics in Adobe After Effects, and character design principles. Workstations provided with Wacom tablets. Collaborative projects will be featured on campus digital displays. No prior art experience required!','Woodlands','2024/11/5 13:00','2024/11/5 16:00','approved',2,'https://example.com/activity/6633',20),(30,'Ultimate Frisbee Tournament','Form teams for our sunset frisbee tournament! All skill levels welcome with beginner rules explanation. We\'ll provide discs and set up fields. Between games, enjoy lawn games and healthy snacks. Winning team gets custom jerseys and bragging rights for the semester.','Yishun','2024/9/18 17:00','2024/9/18 19:00','approved',2,'https://example.com/activity/4267',80),(31,'Science Communication Workshop','Master the art of making science accessible! Practice explaining technical concepts to non-experts using everyday analogies. Through recorded exercises, you\'ll receive feedback on clarity and engagement. Features \'Three Minute Thesis\' style challenges with audience voting for best presentation.','Marina Bay','2024/10/22 15:00','2024/10/22 17:30','approved',2,'https://example.com/activity/8088',40),(32,'Thrift Store DIY Transformation','Transform thrift store items into personalized treasures! Bring 2-3 thrifted pieces and we\'ll provide fabric paints, patches, and sewing supplies. Fashion design students will teach customization techniques from simple embroidery to complete reconstruction. Take home unique pieces and sustainable fashion skills.','Marina Bay','2024/11/19 18:00','2024/11/19 20:30','approved',9,'https://example.com/activity/9980',35),(33,'Indoor Rock Climbing Intro','Experience the thrill of rock climbing in our campus gym! Certified instructors teach belaying techniques, safety procedures, and climbing fundamentals. We provide harnesses and shoes. Try various wall challenges from beginner to advanced routes. Sign up for ongoing climbing club sessions afterward.','Tampines','2024/10/14 16:00','2024/10/14 18:30','approved',9,'https://example.com/activity/6116',28),(34,'Debate Society Open House','Sharpen your critical thinking at our debate showcase! Watch experienced debaters tackle controversial campus topics, then participate in mini-debate workshops. Learn argument structuring, rebuttal techniques, and persuasive delivery. Beginners welcome - great preparation for academic presentations and interviews.','Toa Payoh','2024/9/26 19:00','2024/9/26 21:00','approved',9,'https://example.com/activity/3896',60),(35,'Campus Cleanup & Eco-Chat','Make campus greener while discussing sustainability! We\'ll provide gloves and bags for litter collection, followed by roundtable discussions about campus environmental initiatives. Share ideas for reducing waste in dorms and cafeterias. Participants receive reusable water bottles and snack packs.','Bukit Timah','2024/10/26 10:00','2024/10/26 13:00','approved',6,'https://example.com/activity/6972',9999),(36,'Acoustic Open Mic Night','Share your musical talents at our intimate coffeehouse-style open mic! Perform original songs or covers in a supportive environment. Sign-up slots available with sound system and basic instruments provided. Audience votes for favorite performer who wins recording studio time.','Orchard','2024/11/22 19:30','2024/11/22 22:00','approved',9,'https://example.com/activity/1713',15),(37,'Data Visualization Challenge','Turn datasets into compelling visual stories! Using Tableau and Python libraries, transform provided datasets into insightful visualizations. Learn color theory for data, storytelling techniques, and dashboard design. Top visualizations will be displayed in the library and featured in campus publications.','Marina Bay','2024/10/9 14:00','2024/10/9 17:00','approved',9,'https://example.com/activity/2536',45),(38,'Karaoke Battle Royale','Show off your vocal chops in our Halloween karaoke championship! Costumed performers compete in themed categories (showtunes, 80s hits, current chart-toppers). Professional sound system with thousands of songs. Judges score performance quality and audience reaction. Winner gets recording studio package.','Tampines','2024/10/31 20:00','2024/11/1 0:00','approved',2,'https://example.com/activity/6099',100),(39,'Public Speaking Bootcamp','Transform your presentation skills through practical exercises! Learn voice modulation techniques, body language mastery, and audience engagement strategies. Recorded practice sessions with personalized feedback. Ideal for class presentations, club leadership, and interview preparation.','Changi','2024/9/23 16:00','2024/9/23 18:30','approved',2,'https://example.com/activity/3838',30),(40,'Pottery Wheel Workshop','Get messy with clay in our pottery studio! Learn wheel throwing techniques to create functional pieces like bowls and mugs. Professional potters guide you through centering, shaping, and trimming. Pieces will be glazed and fired for pickup later. Aprons provided - prepare to get creative!','Bukit Timah','2024/11/13 18:00','2024/11/13 20:30','approved',9,'https://example.com/activity/7760',24),(41,'AI Ethics Debate Forum','Engage in critical discussion about AI\'s societal impact! Philosophy and computer science professors moderate debates on bias in algorithms, job displacement concerns, and creative AI. Prepare arguments in breakout groups before full forum discussion. Essential for future tech leaders and policymakers.','Jurong East','2024/10/16 17:00','2024/10/16 19:30','approved',2,'https://example.com/activity/6767',60),(42,'Sunset Yoga Flow','Unwind with gentle yoga as the sun sets! Certified instructors lead accessible sequences focusing on stress release and flexibility. Modifications available for all levels. Mats provided along with lavender-scented towels. Perfect transition from study sessions to evening relaxation.','Woodlands','2024/9/20 18:00','2024/9/20 19:15','approved',2,'https://example.com/activity/1110',75),(43,'Startup Founder Fireside Chats','Hear authentic founder stories in intimate settings! Early-stage entrepreneurs share failures, pivots, and breakthrough moments. Q&A sessions reveal practical startup advice. Network with fellow student founders over pizza and soft drinks. Discover campus entrepreneurship resources and funding opportunities.','Yishun','2024/11/6 18:30','2024/11/6 20:30','approved',2,'https://example.com/activity/2640',50),(44,'Historical Costume Sewing','Step into history through costume creation! Theater department experts teach historical sewing techniques for Regency, Victorian, or 1920s styles. Work with authentic patterns and fabrics to create wearable pieces. No sewing experience needed - we provide machines and guidance.','Tampines','2024/10/24 13:00','2024/10/24 16:00','approved',9,'https://example.com/activity/6493',18),(45,'Language Exchange Cafe','Improve language skills over coffee and pastries! Rotate through conversation tables for Spanish, French, Mandarin, Japanese, and German. Native speakers facilitate discussions with cultural insights. Receive phrase cards and pronunciation tips. Perfect for travelers and language learners.','Woodlands','2024/9/28 17:30','2024/9/28 19:00','approved',9,'https://example.com/activity/1900',60),(46,'Drone Photography Workshop','Master aerial photography with drone technology! Learn FAA regulations, flight safety, and cinematic camera movements. Practice flying provided DJI drones in controlled environments. Editing session teaches color grading and panoramic stitching. Best student shots featured in campus gallery.','Woodlands','2024/11/25 14:00','2024/11/25 17:00','approved',9,'https://example.com/activity/7081',20),(47,'Stress Relief Puppy Therapy','De-stress with certified therapy dogs during exam season! Pet and play with friendly pups in the student center. Learn about animal-assisted therapy and mindfulness techniques. Free snacks and calming tea provided. Take study breaks with the happiest helpers on campus.','Bukit Timah','2024/12/10 12:00','2024/12/10 14:00','approved',2,'https://example.com/activity/4198',9999),(48,'3D Printing Innovation Lab','Bring digital designs to life in our makerspace! Learn CAD modeling basics for 3D printing, then design and print your own functional object (phone stand, jewelry, puzzle). Experts troubleshoot common printing issues. Show off your creations in our innovation showcase.','Yishun','2024/10/2 15:00','2024/10/2 18:00','approved',6,'https://example.com/activity/1803',25),(49,'Journalism & Storytelling Workshop','Develop compelling campus stories! Professional journalists teach interviewing techniques, fact-checking methods, and multimedia storytelling. Pitch stories to campus publications during the workshop. Explore podcasting, video reporting, and digital writing styles. Great for aspiring writers and content creators.','Toa Payoh','2024/11/9 10:00','2024/11/9 13:00','approved',9,'https://example.com/activity/9332',40),(50,'Archery Taster Session','Try the ancient art of archery with certified instructors! Learn proper stance, nocking technique, and safe equipment handling. Practice shooting at targets with recurve bows. Fun competition with prizes for accuracy improvement. Equipment provided - no experience necessary.','Bukit Timah','2024/9/15 13:00','2024/9/15 15:30','approved',6,'https://example.com/activity/7214',30),(51,'Theater Improv for Anxiety','Build social confidence through improv games! Drama therapists guide exercises that reduce social anxiety and encourage spontaneity. Safe environment to practice conversations, public speaking, and quick thinking. Learn techniques applicable to interviews, dates, and classroom participation.','Tampines','2024/10/30 16:00','2024/10/30 18:00','approved',6,'https://example.com/activity/6377',25),(52,'Board Game Design Challenge','Design and prototype your own board game! Learn mechanics, balance testing, and theme development. Create components using our craft supplies and simple electronics. Playtest other student creations and provide feedback. Top three games get professional development advice from game designers.','Jurong East','2024/11/16 12:00','2024/11/16 16:00','approved',2,'https://example.com/activity/5651',35),(53,'Neuroscience of Learning Talk','Discover brain-based study strategies! Neuroscience professors explain memory formation, focus optimization, and effective learning techniques. Interactive demonstrations show how sleep, nutrition, and exercise impact academic performance. Receive personalized study plan templates based on cognitive science.','Orchard','2024/10/4 18:00','2024/10/4 20:00','approved',9,'https://example.com/activity/9610',90),(54,'Urban Dance Workshop','Learn popping, locking, and breaking fundamentals! Professional dancers teach foundational moves and short choreography sequences. Explore dance history and cultural context. Comfortable clothing recommended - no special shoes required. Show off new skills in open dance jam session.','Yishun','2024/9/29 15:00','2024/9/29 17:00','approved',6,'https://example.com/activity/8753',40),(55,'Robotics Build Challenge','Build functional robots in teams of three! Using Arduino kits and sensors, create robots that complete obstacle courses. Learn basic circuitry, mechanical assembly, and block-based programming. Competition categories for speed, creativity, and technical complexity. Take home your creation!','Changi','2024/11/7 13:00','2024/11/7 17:00','approved',9,'https://example.com/activity/9214',30),(56,'Poetry Slam & Open Mic','Share your poetry in a supportive space! Featured poets perform before open mic sessions. Writing prompts and improvisation exercises for beginners. Themes include identity, social justice, and campus life. Comfortable lounge setting with tea and cookies.','Woodlands','2024/10/23 19:00','2024/10/23 21:30','approved',2,'https://example.com/activity/7423',50),(57,'First Aid & CPR Certification','Get certified in essential emergency response! Red Cross instructors teach adult/child/infant CPR, AED use, and choking response. Practice bandaging, splinting, and emergency scenario management. Certification valid for two years - essential for campus jobs and leadership positions.','Marina Bay','2024/10/11 9:00','2024/10/11 16:00','approved',6,'https://example.com/activity/6767',20),(58,'Graphic Design Bootcamp','Master design principles in this intensive workshop! Create logos, posters, and digital graphics using Adobe Creative Suite. Learn typography pairing, color theory, and composition techniques. Work on real campus organization projects for your portfolio. Laptops with software provided.','Orchard','2024/11/14 10:00','2024/11/14 15:00','approved',2,'https://example.com/activity/7958',25),(59,'Forest Therapy Walk','Experience Japanese shinrin-yoku (forest bathing) in campus woodlands! Guided sensory exercises deepen connection with nature. Practice mindfulness among trees while learning about local ecology. Journaling prompts help process stress and gain perspective. Dress for light hiking - rain or shine.','Yishun','2024/10/1 9:00','2024/10/1 12:00','approved',6,'https://example.com/activity/6675',35),(60,'Cultural Cooking Class','Hands-on cooking with international students! Each month features a different cuisine: Thai curries, Mexican tamales, Italian pasta from scratch. Learn about cultural traditions while chopping and stirring. Eat your creations family-style. Recipe cards and spice samples included.','Clementi','2024/11/21 17:00','2024/11/21 20:00','approved',9,'https://example.com/activity/9095',24),(61,'Personal Branding Workshop','Craft your professional digital identity! Develop consistent branding across LinkedIn, portfolios, and social media. Learn personal storytelling, headshot best practices, and content strategy. Create actionable plans to showcase your unique strengths to employers and networks.','Bukit Timah','2024/10/17 16:00','2024/10/17 18:30','approved',2,'https://example.com/activity/6764',40),(62,'Night Photography Expedition','Master low-light photography techniques! Explore campus architecture with tripods and long exposures. Learn light painting, star trail photography, and urban night composition. DSLRs available for loan or bring your own camera. Best photos featured in campus night gallery.','Clementi','2024/11/4 19:00','2024/11/4 22:00','approved',9,'https://example.com/activity/1887',30),(63,'Graduate School Panel','Plan your graduate education journey! Current PhD candidates and recent graduates share application strategies, funding tips, and program selection advice. Breakout sessions for STEM, humanities, and professional degrees. Get your personal statement reviewed and network with program representatives.','Yishun','2024/9/27 17:00','2024/9/27 19:00','approved',6,'https://example.com/activity/1890',75),(64,'Blockchain Basics Workshop','Learn blockchain fundamentals through interactive coding exercises. Create your first smart contract and understand cryptocurrency principles. No prior experience needed.','Tampines','2024/11/18 13:00','2024/11/18 15:30','approved',2,'https://example.com/activity/1977',35),(65,'Marine Biology Excursion','Field trip to local marine reserve with biology professors. Collect water samples, identify species, and discuss conservation challenges. Transportation and equipment provided.','Yishun','2024/10/8 8:00','2024/10/8 16:00','approved',9,'https://example.com/activity/9883',25),(66,'Digital Marketing Strategies','Master SEO, social media advertising, and content marketing. Create a digital campaign for a campus event and analyze results.','Changi','2024/10/19 14:00','2024/10/19 17:00','approved',9,'https://example.com/activity/6873',40),(67,'Origami Art Therapy','Combine mindfulness with creative paper folding. Learn therapeutic origami techniques to improve focus and relaxation.','Marina Bay','2024/11/12 18:30','2024/11/12 20:30','approved',2,'https://example.com/activity/1632',9999),(68,'App Development Crash Course','Create your first mobile app using React Native. Design UI/UX and implement core functionality with instructor guidance.','Marina Bay','2024/10/28 10:00','2024/10/28 16:00','approved',2,'https://example.com/activity/6530',30),(69,'Entrepreneurial Finance','Learn valuation methods, pitch deck financials, and investor negotiation tactics from successful founders.','Yishun','2024/11/20 16:00','2024/11/20 18:30','approved',2,'https://example.com/activity/3103',45),(70,'Birdwatching Society','Morning birdwatching expedition with ornithology experts. Binoculars and field guides provided.','Yishun','2024/10/15 7:00','2024/10/15 9:00','approved',6,'https://example.com/activity/3565',20),(71,'Virtual Reality Art Studio','Use VR tools to model and paint in three-dimensional space. Export creations for 3D printing.','Orchard','2024/11/26 13:00','2024/11/26 16:00','approved',6,'https://example.com/activity/1408',15),(72,'Leadership Retreat','Team-building exercises and leadership theory sessions in nature setting. For club officers and aspiring leaders.','Clementi','2024/10/10 9:00','2024/10/10 17:00','approved',6,'https://example.com/activity/3706',25),(73,'Quantum Computing Intro','Demystify quantum bits and gates. Hands-on with cloud-based quantum processors.','Marina Bay','2024/11/23 14:00','2024/11/23 16:30','approved',6,'https://example.com/activity/7667',30),(74,'Sustainable Architecture Tour','Visit LEED-certified campus buildings with architects. Discuss renewable materials and energy efficiency.','Yishun','2024/10/27 10:00','2024/10/27 12:00','approved',2,'https://example.com/activity/8319',40),(75,'Creative Coding with Processing','Generate visual art through code. Learn algorithmic art principles using Processing language.','Tampines','2024/11/27 18:00','2024/11/27 20:30','approved',6,'https://example.com/activity/2792',25),(76,'Biotech Career Panel','Learn about career paths in pharmaceuticals, genetics, and medical devices from industry experts.','Yishun','2024/10/21 17:00','2024/10/21 19:00','approved',6,'https://example.com/activity/1618',50),(77,'Mindful Movement','Gentle qigong and tai chi sequences for morning energy. Focus on breath-body coordination.','Clementi','2024/11/28 8:00','2024/11/28 9:30','approved',9,'https://example.com/activity/1976',9999),(78,'Game Theory Workshop','Apply game theory concepts to real-world scenarios. Prisoner\'s dilemma and Nash equilibrium exercises.','Toa Payoh','2024/10/30 14:00','2024/10/30 16:30','approved',6,'https://example.com/activity/3254',35),(79,'Herbal Medicine Making','Harvest campus herbs and prepare tinctures, salves, and teas. Learn medicinal plant properties.','Orchard','2024/11/29 15:00','2024/11/29 17:30','approved',6,'https://example.com/activity/5925',20),(80,'Data Science Hackathon','Solve real-world problems with datasets from local nonprofits. Prizes for best insights and visualizations.','Yishun','2024/10/31 12:00','2024/11/1 12:00','approved',9,'https://example.com/activity/1436',60),(81,'Screenwriting Intensive','Structure compelling narratives, develop characters, and write dialogue. Pitch session with film students.','Changi','2024/12/1 10:00','2024/12/1 16:00','approved',6,'https://example.com/activity/7586',25),(82,'Circular Economy Forum','Discuss campus initiatives for zero waste. Design product lifecycles with sustainability experts.','Jurong East','2024/11/1 13:00','2024/11/1 15:30','approved',6,'https://example.com/activity/4967',40),(83,'Comic Book Creation','Learn panel composition, inking techniques, and character design. Create a 4-page mini-comic.','Jurong East','2024/12/2 18:00','2024/12/2 20:30','approved',6,'https://example.com/activity/2667',30),(84,'Investment Club Meeting','Analyze companies, practice portfolio management, and discuss market trends. Beginners welcome.','Woodlands','2024/11/2 19:00','2024/11/2 21:00','approved',2,'https://example.com/activity/6374',9999),(85,'Astrophotography Night','Photograph moon, planets, and stars with DSLRs on tracking mounts. Basic editing instruction included.','Yishun','2024/12/3 20:00','2024/12/3 23:00','approved',6,'https://example.com/activity/9037',20),(86,'Public Policy Debate','Simulate congressional sessions debating real bills. Research positions and practice persuasive arguments.','Marina Bay','2024/11/3 16:00','2024/11/3 18:30','approved',2,'https://example.com/activity/3616',45),(87,'Traditional Woodworking','Learn joinery techniques using hand tools. Create a small wooden box or cutting board.','Clementi','2024/12/4 13:00','2024/12/4 16:00','approved',9,'https://example.com/activity/2143',15),(88,'Neuroscience Research Symposium','Showcase undergraduate neuroscience research. Keynote from leading cognitive scientist.','Changi','2024/11/4 9:00','2024/11/4 12:00','approved',6,'https://example.com/activity/9336',100),(89,'Sustainable Aquaponics','Construct small-scale aquaponic systems. Learn symbiotic relationships between fish and plants.','Yishun','2024/12/5 14:00','2024/12/5 17:00','approved',9,'https://example.com/activity/8588',25),(90,'Ethical Hacking Workshop','Practice penetration testing in controlled environment. Learn defense strategies against common attacks.','Jurong East','2024/11/5 18:00','2024/11/5 21:00','approved',9,'https://example.com/activity/3053',30),(91,'Indigenous Art Workshop','Create beadwork or basket weaving with native artists. Discuss cultural significance of patterns.','Tampines','2024/12/6 11:00','2024/12/6 14:00','approved',6,'https://example.com/activity/4729',20),(92,'Biomedical Engineering Demo','Design assistive devices using 3D printing. Test prototypes with accessibility advocates.','Clementi','2024/11/6 13:00','2024/11/6 16:00','approved',2,'https://example.com/activity/9378',35),(93,'Poetry Therapy Session','Write and share poetry for emotional processing. Guided exercises for self-expression and reflection.','Yishun','2024/12/7 15:00','2024/12/7 17:00','approved',9,'https://example.com/activity/4915',9999),(94,'Urban Planning Simulation','Use software to balance housing, transit, and green spaces. Present plans to mock city council.','Bukit Timah','2024/11/7 10:00','2024/11/7 13:00','approved',9,'https://example.com/activity/6397',40),(95,'Classical Music Appreciation','Live string quartet performance followed by discussion of musical structures and historical context.','Clementi','2024/12/8 19:00','2024/12/8 21:30','approved',6,'https://example.com/activity/1626',60),(96,'Behavioral Economics Talk','Explore cognitive biases in economic choices. Interactive experiments on risk perception.','Orchard','2024/11/8 17:00','2024/11/8 19:00','approved',9,'https://example.com/activity/8078',50),(97,'Glass Blowing Introduction','Create glass ornaments with master craftsmen. Safety equipment provided.','Yishun','2024/12/9 13:00','2024/12/9 16:00','approved',2,'https://example.com/activity/5014',12),(98,'Climate Science Panel','Interdisciplinary discussion on climate mitigation strategies. Q&A with environmental scientists.','Woodlands','2024/11/9 14:00','2024/11/9 16:30','approved',9,'https://example.com/activity/1867',75),(99,'Film Scoring Workshop','Create emotional soundscapes for film clips. Use digital audio workstations and sample libraries.','Toa Payoh','2024/12/10 18:00','2024/12/10 21:00','approved',6,'https://example.com/activity/9915',20),(100,'Robotic Surgery Demo','Operate training surgical robots. Discussion on AI in medicine with healthcare professionals.','Woodlands','2024/11/10 10:00','2024/11/10 12:00','approved',6,'https://example.com/activity/1220',30),(101,'Anthropology Field Methods','Practice ethnographic observation and interviewing in campus settings. Ethics of cultural research.','Clementi','2024/12/11 9:00','2024/12/11 12:00','approved',2,'https://example.com/activity/6578',25),(102,'Startup Legal Basics','Intellectual property, incorporation, and contracts for student entrepreneurs. Q&A with startup lawyers.','Marina Bay','2024/11/11 15:00','2024/11/11 17:30','approved',9,'https://example.com/activity/9559',40),(103,'Experimental Photography','Create cyanotypes, chemigrams, and lumen prints. Explore camera-less photography techniques.','Orchard','2024/12/12 14:00','2024/12/12 17:00','approved',9,'https://example.com/activity/7164',18),(104,'Disability Advocacy Training','Learn inclusive design principles and communication strategies. Campus accessibility audit exercise.','Marina Bay','2024/11/12 11:00','2024/11/12 13:30','approved',9,'https://example.com/activity/3076',9999),(105,'Geology Field Trip','Visit local geological sites. Identify rock types and discuss earth history.','Tampines','2024/12/13 8:00','2024/12/13 16:00','approved',6,'https://example.com/activity/2724',30),(106,'Social Media Analytics','Track campaign performance and interpret metrics. Hands-on with analytics platforms.','Yishun','2024/11/13 14:00','2024/11/13 16:30','approved',6,'https://example.com/activity/2095',35),(107,'Book Binding Workshop','Learn stitching techniques for hardcover books. Decorate covers with personal designs.','Jurong East','2024/12/14 13:00','2024/12/14 15:30','approved',2,'https://example.com/activity/6084',20),(108,'Microbiome Science Talk','Latest research on microbiome health. Q&A with microbiology researchers.','Toa Payoh','2024/11/14 18:00','2024/11/14 20:00','approved',9,'https://example.com/activity/5097',60),(109,'Jazz Improvisation Class','Learn scales, chord changes, and soloing techniques. Jam with professional musicians.','Changi','2024/12/15 19:00','2024/12/15 21:00','approved',2,'https://example.com/activity/9099',25),(110,'Renewable Energy Lab','Build simple renewable energy systems. Measure output efficiency under different conditions.','Toa Payoh','2024/11/15 13:00','2024/11/15 16:00','approved',6,'https://example.com/activity/1981',30),(111,'Stand-Up Comedy Workshop','Craft jokes, develop timing, and overcome stage fright. Perform at campus comedy night.','Changi','2024/12/16 17:00','2024/12/16 19:30','approved',9,'https://example.com/activity/2500',20),(112,'Forensic Science Demo','Analyze mock crime scenes. Fingerprinting, fiber analysis, and blood spatter patterns.','Bukit Timah','2024/11/16 10:00','2024/11/16 12:30','approved',2,'https://example.com/activity/6480',40),(113,'Community Mural Painting','Collaborate on large-scale mural for campus wall. Contribute to design and painting.','Bukit Timah','2024/12/17 11:00','2024/12/17 16:00','approved',6,'https://example.com/activity/8027',9999),(114,'Clinical Psychology Seminar','Psychologists discuss career options in mental health fields. Q&A session and internship opportunities.','Bukit Timah','2025/1/15 14:00','2025/1/15 16:30','approved',2,'https://example.com/activity/4600',45),(115,'Digital Currency Trading','Technical analysis and risk management for cryptocurrency trading. Demo on trading platforms.','Orchard','2025/1/10 19:00','2025/1/10 21:30','approved',9,'https://example.com/activity/7021',35),(116,'Wildlife Photography Hike','Morning expedition to photograph birds and mammals. Composition and stealth techniques.','Changi','2025/1/22 6:00','2025/1/22 9:00','approved',6,'https://example.com/activity/3280',15),(117,'Social Entrepreneurship','Develop business models that solve social problems. Case studies of successful social enterprises.','Bukit Timah','2025/1/17 13:00','2025/1/17 15:30','approved',2,'https://example.com/activity/2129',40),(118,'Creative Nonfiction Writing','Transform personal experiences into compelling narratives. Memoir and literary journalism techniques.','Tampines','2025/1/29 18:00','2025/1/29 20:30','approved',2,'https://example.com/activity/5749',25),(119,'Augmented Reality Demo','Create AR experiences for campus landmarks using mobile devices. No coding required.','Clementi','2025/1/12 14:00','2025/1/12 16:00','approved',9,'https://example.com/activity/7746',30),(120,'Nutrition Science Workshop','Evidence-based nutrition guidelines. Debunk diet myths with scientific research.','Yishun','2025/1/24 17:00','2025/1/24 19:00','approved',2,'https://example.com/activity/7825',50),(121,'Historical Reenactment','Recreate historical events with period clothing and props. Civil War and Renaissance options.','Bukit Timah','2025/1/19 11:00','2025/1/19 14:00','approved',6,'https://example.com/activity/5738',9999),(122,'Mobile Game Design','Design addictive game mechanics. Prototype simple games with drag-and-drop tools.','Marina Bay','2025/1/31 13:00','2025/1/31 16:00','approved',2,'https://example.com/activity/5160',25),(123,'Environmental Law Talk','Landmark environmental cases and current regulations. Career paths in environmental law.','Clementi','2025/1/14 15:00','2025/1/14 17:30','approved',2,'https://example.com/activity/7852',40),(124,'Fashion Illustration','Master figure drawing for fashion. Render fabrics and textures with various media.','Toa Payoh','2025/1/26 10:00','2025/1/26 12:30','approved',9,'https://example.com/activity/2448',20),(125,'Volunteer Management','Recruit, train, and retain volunteers. Strategies for nonprofit organizations.','Marina Bay','2025/1/21 18:00','2025/1/21 20:00','approved',2,'https://example.com/activity/4631',35),(126,'Space Colonization Debate','Discuss challenges of interplanetary habitation. Teams debate pros and cons.','Jurong East','2025/2/2 16:00','2025/2/2 18:30','approved',2,'https://example.com/activity/8914',60),(127,'Botanical Illustration','Combine art and science in detailed plant drawings. Field sketching techniques.','Changi','2025/1/28 14:00','2025/1/28 16:30','approved',6,'https://example.com/activity/9055',18),(128,'User Experience Design','Conduct user research, prototype interfaces, and run usability tests. Portfolio project.','Changi','2025/2/4 13:00','2025/2/4 16:00','approved',6,'https://example.com/activity/4313',30),(129,'Mental Health First Aid','Recognize signs of mental distress. Practice supportive conversations and resource referral.','Clementi','2025/1/23 10:00','2025/1/23 16:00','approved',6,'https://example.com/activity/3713',9999),(130,'Podcast Sound Design','Foley techniques, ambient soundscapes, and audio processing. Enhance storytelling with sound.','Bukit Timah','2025/2/6 18:00','2025/2/6 20:30','approved',9,'https://example.com/activity/7616',25),(131,'Sustainable Fashion Design','Work with organic cotton, recycled polyester, and natural dyes. Zero-waste pattern cutting.','Clementi','2025/1/30 14:00','2025/1/30 17:00','approved',2,'https://example.com/activity/1638',20),(132,'Philosophy Cafe','Socratic dialogues on ethics, existence, and knowledge. No background required.','Tampines','2025/2/8 19:00','2025/2/8 21:00','approved',6,'https://example.com/activity/1059',9999),(133,'Circus Skills Workshop','Learn basic juggling, plate spinning, and balance techniques. Fun physical challenge.','Woodlands','2025/1/25 16:00','2025/1/25 18:30','approved',9,'https://example.com/activity/5266',40),(134,'Genomic Sequencing Demo','Extract and sequence DNA samples. Discuss ethical implications of genetic testing.','Tampines','2025/2/10 14:00','2025/2/10 17:00','approved',9,'https://example.com/activity/9672',25),(135,'Community Poetry Wall','Contribute lines to evolving campus poem. Magnetic poetry and typewriter stations.','Changi','2025/2/1 12:00','2025/2/1 14:00','approved',2,'https://example.com/activity/6019',9999),(136,'Disaster Response Training','Simulate natural disaster scenarios. Triage, resource allocation, and crisis communication.','Jurong East','2025/2/12 9:00','2025/2/12 16:00','approved',6,'https://example.com/activity/2697',35),(137,'Ethnobotany Walk','Campus plants with traditional uses. Sustainable harvesting ethics.','Bukit Timah','2025/2/3 10:00','2025/2/3 12:00','approved',2,'https://example.com/activity/1611',20),(138,'Animation Storyboarding','Create shot sequences for animation. Learn cinematography principles for 2D and 3D.','Yishun','2025/2/14 13:00','2025/2/14 16:00','approved',6,'https://example.com/activity/8939',30),(139,'Financial Aid Workshop','Navigate scholarships, grants, and loan options. Application tips from financial aid officers.','Yishun','2025/2/5 17:00','2025/2/5 19:00','approved',6,'https://example.com/activity/2859',50),(140,'Archery Tag Tournament','Team-based archery competition with foam-tipped arrows. Safety equipment provided.','Yishun','2025/2/16 14:00','2025/2/16 17:00','approved',6,'https://example.com/activity/8666',40),(141,'Science Fiction Book Club','Monthly discussion of classic and contemporary sci-fi. Author analysis and theme exploration.','Woodlands','2025/2/7 18:00','2025/2/7 20:00','approved',2,'https://example.com/activity/3156',25),(142,'Biomimicry Design','Apply biological solutions to engineering challenges. Case studies and design exercises.','Bukit Timah','2025/2/18 15:00','2025/2/18 17:30','approved',9,'https://example.com/activity/7055',30),(143,'Cultural Appropriation Talk','Distinguish appreciation from appropriation. Panel with cultural studies scholars.','Bukit Timah','2025/2/9 16:00','2025/2/9 18:30','approved',9,'https://example.com/activity/1994',60),(144,'Parkour Fundamentals','Safe vaulting, rolling, and climbing techniques. Build confidence and spatial awareness.','Woodlands','2025/2/20 10:00','2025/2/20 12:30','approved',9,'https://example.com/activity/7481',20),(145,'Art Therapy for Trauma','Expressive arts for processing difficult experiences. Guided by licensed art therapists.','Clementi','2025/2/11 14:00','2025/2/11 16:30','approved',9,'https://example.com/activity/1264',15),(146,'Quantum Physics Demo','Hands-on demonstrations of quantum phenomena. Discussion of interpretations and implications.','Orchard','2025/2/22 18:00','2025/2/22 20:00','approved',9,'https://example.com/activity/1106',40),(147,'Food Justice Forum','Address food deserts, worker rights, and sustainable agriculture. Action planning session.','Bukit Timah','2025/2/13 12:00','2025/2/13 14:30','approved',6,'https://example.com/activity/3511',50),(148,'Silent Disco Dance Party','Three-channel silent disco with different music genres. Glow accessories and refreshments.','Clementi','2025/2/24 20:00','2025/2/24 23:00','approved',2,'https://example.com/activity/5805',100),(149,'Patent Law Basics','Navigating patent process for student innovators. Prior art search and application drafting.','Yishun','2025/2/15 15:00','2025/2/15 17:00','approved',6,'https://example.com/activity/7075',30),(150,'Guerrilla Gardening','Create seed bombs for neglected urban spaces. Native wildflower mix preparation.','Changi','2025/2/26 14:00','2025/2/26 16:00','approved',6,'https://example.com/activity/9482',9999),(151,'Musical Theater Audition','Prepare songs and monologues. Feedback from theater directors and casting agents.','Woodlands','2025/2/17 18:00','2025/2/17 21:00','approved',2,'https://example.com/activity/2669',25),(152,'Paleontology Lab','Clean and catalog real fossils. Discuss prehistoric ecosystems.','Tampines','2025/2/28 13:00','2025/2/28 16:00','approved',6,'https://example.com/activity/5693',20),(153,'Mind Mapping Workshop','Organize ideas and boost creativity. Applications for studying and project planning.','Orchard','2025/2/19 10:00','2025/2/19 12:00','approved',2,'https://example.com/activity/6961',35),(154,'Beekeeping Society','Inspect campus hives, harvest honey, and learn bee behavior. Protective suits provided.','Yishun','2025/3/2 9:00','2025/3/2 11:00','approved',6,'https://example.com/activity/4960',15),(155,'Video Essay Creation','Combine research, writing, and editing to make compelling video arguments.','Jurong East','2025/2/21 14:00','2025/2/21 17:00','approved',2,'https://example.com/activity/8630',30),(156,'Restorative Justice Circle','Facilitated dialogue for community healing. Based on indigenous peacemaking traditions.','Marina Bay','2025/3/4 16:00','2025/3/4 18:30','approved',6,'https://example.com/activity/4542',25),(157,'Experimental Music Lab','Create unconventional instruments and compositions. Noise, ambient, and electronic techniques.','Jurong East','2025/2/23 19:00','2025/2/23 21:30','approved',2,'https://example.com/activity/8789',20),(158,'Green Chemistry Demo','Environmentally benign chemical synthesis. Reduce waste and hazardous materials.','Tampines','2025/3/6 13:00','2025/3/6 15:00','approved',6,'https://example.com/activity/4179',35),(159,'Immersive Theater Experience','Audience-participation theater exploring social themes. Multiple narrative pathways.','Woodlands','2025/2/25 18:00','2025/2/25 21:00','approved',2,'https://example.com/activity/4653',40),(160,'Astrology & Astronomy','Compare scientific and cultural understandings of celestial phenomena. Planetarium show included.','Woodlands','2025/3/8 19:00','2025/3/8 21:00','approved',2,'https://example.com/activity/6300',50),(161,'Entrepreneurial Marketing','Growth hacking for startups. Viral strategies and lean marketing tactics.','Orchard','2025/2/27 10:00','2025/2/27 12:30','approved',2,'https://example.com/activity/8407',45),(162,'Zen Garden Creation','Design and build miniature zen gardens. Principles of Japanese garden aesthetics.','Orchard','2025/3/10 14:00','2025/3/10 16:30','approved',2,'https://example.com/activity/1065',20),(163,'Clinical Psychology Seminar','Psychologists discuss career options in mental health fields. Q&A session and internship opportunities.','Jurong East','2025/1/15 14:00','2025/1/15 16:30','approved',2,'https://example.com/activity/5791',45);
/*!40000 ALTER TABLE `activities` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `activityregistrationrequests`
--

DROP TABLE IF EXISTS `activityregistrationrequests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `activityregistrationrequests` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `ActivityId` int NOT NULL,
  `Status` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RequestedAt` datetime(6) NOT NULL,
  `ReviewedAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ActivityRegistrationRequests_ActivityId` (`ActivityId`),
  KEY `IX_ActivityRegistrationRequests_UserId` (`UserId`),
  CONSTRAINT `FK_ActivityRegistrationRequests_Activities_ActivityId` FOREIGN KEY (`ActivityId`) REFERENCES `activities` (`ActivityId`) ON DELETE CASCADE,
  CONSTRAINT `FK_ActivityRegistrationRequests_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `activityregistrationrequests`
--

LOCK TABLES `activityregistrationrequests` WRITE;
/*!40000 ALTER TABLE `activityregistrationrequests` DISABLE KEYS */;
INSERT INTO `activityregistrationrequests` VALUES (1,3,1,'approved','2025-07-29 07:02:03.428489','2025-08-05 04:41:10.714457'),(2,9,1,'approved','2025-08-04 08:11:00.092196','2025-08-05 04:41:34.367609'),(5,4,4,'approved','2025-08-06 03:01:18.734583','2025-08-06 07:27:55.108872');
/*!40000 ALTER TABLE `activityregistrationrequests` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `activityrequest`
--

DROP TABLE IF EXISTS `activityrequest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `activityrequest` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ActivityId` int NOT NULL,
  `ReviewedById` int NOT NULL,
  `requestType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Status` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RequestedAt` datetime(6) NOT NULL,
  `ReviewedAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ActivityRequest_ActivityId` (`ActivityId`),
  KEY `IX_ActivityRequest_ReviewedById` (`ReviewedById`),
  CONSTRAINT `FK_ActivityRequest_Activities_ActivityId` FOREIGN KEY (`ActivityId`) REFERENCES `activities` (`ActivityId`) ON DELETE CASCADE,
  CONSTRAINT `FK_ActivityRequest_Users_ReviewedById` FOREIGN KEY (`ReviewedById`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `activityrequest`
--

LOCK TABLES `activityrequest` WRITE;
/*!40000 ALTER TABLE `activityrequest` DISABLE KEYS */;
INSERT INTO `activityrequest` VALUES (1,1,2,'createActivity','approved','2025-07-29 06:55:12.036933','2025-07-29 06:55:52.029214'),(2,2,2,'createActivity','approved','2025-07-29 06:55:20.859854','2025-07-29 06:55:54.962498'),(3,3,2,'createActivity','approved','2025-07-29 06:55:28.223766','2025-07-29 06:55:56.821430'),(4,4,2,'createActivity','approved','2025-08-01 07:23:48.939184','2025-08-01 07:24:48.856838'),(5,5,2,'createActivity','approved','2025-08-01 07:29:44.760231','2025-08-01 07:30:32.726768'),(6,6,2,'createActivity','rejected','2025-08-01 07:31:40.056655','2025-08-01 07:33:05.568410'),(7,7,2,'createActivity','approved','2025-08-02 05:54:58.088915','2025-08-06 04:51:52.077760'),(8,8,2,'createActivity','approved','2025-08-04 08:12:00.662662','2025-08-06 04:53:01.611640'),(9,9,2,'createActivity','rejected','2025-08-04 08:31:59.437729','2025-08-06 04:53:25.583526'),(10,10,2,'createActivity','pending','2025-08-04 08:57:08.150223',NULL),(11,11,2,'createActivity','approved','2025-08-04 09:09:42.877037','2025-08-06 07:45:58.542808'),(12,2,1,'updateActivity','approved','2025-08-06 07:22:31.946410','2025-08-06 07:24:18.155664'),(13,12,2,'createActivity','pending','2025-08-06 07:27:38.862420',NULL),(14,5,1,'updateActivity','approved','2025-08-06 07:28:06.691747','2025-08-07 03:41:06.613706'),(16,1,1,'updateActivity','pending','2025-08-07 03:37:20.060959',NULL);
/*!40000 ALTER TABLE `activityrequest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `activitytag`
--

DROP TABLE IF EXISTS `activitytag`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `activitytag` (
  `ActivityId` int NOT NULL,
  `TagId` int NOT NULL,
  PRIMARY KEY (`ActivityId`,`TagId`),
  KEY `IX_ActivityTag_TagId` (`TagId`),
  CONSTRAINT `FK_ActivityTag_Activities_ActivityId` FOREIGN KEY (`ActivityId`) REFERENCES `activities` (`ActivityId`) ON DELETE CASCADE,
  CONSTRAINT `FK_ActivityTag_Tags_TagId` FOREIGN KEY (`TagId`) REFERENCES `tags` (`TagId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `activitytag`
--

LOCK TABLES `activitytag` WRITE;
/*!40000 ALTER TABLE `activitytag` DISABLE KEYS */;
INSERT INTO `activitytag` VALUES (1,10),(23,10),(24,10),(28,10),(31,10),(34,10),(37,10),(41,10),(53,10),(63,10),(65,10),(73,10),(78,10),(86,10),(88,10),(95,10),(96,10),(98,10),(101,10),(105,10),(108,10),(112,10),(114,10),(132,10),(139,10),(141,10),(146,10),(152,10),(153,10),(155,10),(163,10),(1,11),(2,11),(101,11),(137,11),(3,12),(18,12),(29,12),(40,12),(44,12),(58,12),(71,12),(75,12),(83,12),(91,12),(97,12),(103,12),(107,12),(113,12),(124,12),(127,12),(138,12),(4,13),(11,13),(67,13),(145,13),(5,14),(12,14),(22,14),(36,14),(56,14),(62,14),(111,14),(135,14),(151,14),(159,14),(6,15),(85,15),(126,15),(160,15),(7,16),(9,16),(130,16),(8,17),(89,17),(108,17),(127,17),(134,17),(142,17),(76,18),(82,19),(117,19),(16,20),(23,20),(28,20),(39,20),(43,20),(61,20),(63,20),(66,20),(69,20),(76,20),(102,20),(106,20),(114,20),(123,20),(139,20),(149,20),(151,20),(161,20),(163,20),(57,21),(158,22),(14,23),(20,23),(37,23),(64,23),(68,23),(75,23),(80,23),(90,23),(31,24),(39,24),(49,24),(111,24),(35,25),(113,25),(125,25),(135,25),(150,25),(156,25),(15,26),(25,26),(60,27),(114,28),(145,28),(163,28),(32,29),(40,29),(44,29),(48,29),(67,29),(79,29),(87,29),(91,29),(97,29),(107,29),(20,30),(26,30),(29,30),(46,30),(52,30),(75,30),(99,30),(103,30),(122,30),(130,30),(157,30),(56,31),(81,31),(83,31),(93,31),(118,31),(135,31),(112,32),(95,33),(121,33),(160,33),(91,34),(143,34),(54,35),(80,36),(106,36),(34,37),(86,37),(126,37),(37,38),(52,38),(58,38),(74,38),(87,38),(92,38),(94,38),(119,38),(124,38),(128,38),(131,38),(138,38),(142,38),(162,38),(35,39),(82,39),(150,39),(78,40),(96,40),(55,41),(92,41),(100,41),(110,41),(142,41),(16,42),(43,42),(69,42),(102,42),(117,42),(149,42),(161,42),(65,43),(74,43),(94,43),(98,43),(123,43),(41,44),(126,44),(134,44),(143,44),(157,45),(124,46),(131,46),(81,47),(99,47),(138,47),(155,47),(84,48),(115,48),(139,48),(33,49),(54,49),(77,49),(133,49),(144,49),(19,50),(60,50),(147,50),(122,51),(25,52),(52,52),(21,53),(89,53),(150,53),(162,53),(105,54),(86,55),(152,56),(154,56),(27,57),(57,57),(92,57),(100,57),(108,57),(120,57),(129,57),(44,58),(121,58),(83,59),(104,60),(119,61),(159,62),(19,63),(45,63),(60,63),(69,64),(84,64),(115,64),(118,65),(45,66),(102,67),(123,67),(149,67),(72,68),(104,68),(125,68),(136,68),(156,68),(66,69),(106,69),(161,69),(26,70),(49,70),(130,70),(155,70),(17,71),(59,72),(77,72),(68,73),(36,74),(38,74),(95,74),(99,74),(109,74),(148,74),(157,74),(17,75),(24,75),(59,75),(65,75),(70,75),(79,75),(85,75),(105,75),(116,75),(127,75),(137,75),(154,75),(43,76),(61,76),(63,76),(76,76),(88,77),(111,78),(133,78),(151,78),(72,79),(132,80),(46,81),(62,81),(85,81),(103,81),(116,81),(73,82),(110,82),(146,82),(153,83),(96,84),(141,85),(10,86),(22,86),(25,86),(30,86),(33,86),(38,86),(42,86),(50,86),(62,86),(70,86),(109,86),(121,86),(133,86),(140,86),(144,86),(148,86),(88,87),(101,87),(23,88),(27,89),(57,89),(129,89),(136,89),(120,90),(152,90),(158,90),(90,91),(19,92),(22,92),(24,92),(34,92),(36,92),(38,92),(45,92),(47,92),(51,92),(54,92),(56,92),(132,92),(141,92),(143,92),(148,92),(160,92),(147,93),(15,94),(30,94),(33,94),(50,94),(140,94),(144,94),(47,95),(18,96),(21,96),(32,96),(35,96),(74,96),(82,96),(89,96),(98,96),(110,96),(117,96),(131,96),(147,96),(154,96),(158,96),(15,97),(14,98),(30,98),(55,98),(72,98),(80,98),(113,98),(140,98),(41,99),(64,99),(68,99),(73,99),(90,99),(100,99),(115,99),(122,99),(128,99),(134,99),(146,99),(14,100),(20,100),(29,100),(46,100),(48,100),(55,100),(71,100),(119,100),(159,101),(93,102),(18,103),(71,104),(17,105),(21,105),(42,105),(47,105),(50,105),(51,105),(53,105),(59,105),(67,105),(77,105),(79,105),(93,105),(120,105),(129,105),(137,105),(145,105),(162,105),(70,106),(116,106),(16,107),(26,107),(27,107),(28,107),(31,107),(32,107),(39,107),(40,107),(48,107),(51,107),(53,107),(58,107),(61,107),(64,107),(66,107),(78,107),(81,107),(84,107),(87,107),(94,107),(97,107),(104,107),(107,107),(109,107),(112,107),(118,107),(125,107),(128,107),(136,107),(153,107),(156,107),(49,108),(42,109);
/*!40000 ALTER TABLE `activitytag` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `activityuser`
--

DROP TABLE IF EXISTS `activityuser`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `activityuser` (
  `ActivityId` int NOT NULL,
  `UserId` int NOT NULL,
  PRIMARY KEY (`ActivityId`,`UserId`),
  KEY `IX_ActivityUser_UserId` (`UserId`),
  CONSTRAINT `FK_ActivityUser_Activities_ActivityId` FOREIGN KEY (`ActivityId`) REFERENCES `activities` (`ActivityId`) ON DELETE CASCADE,
  CONSTRAINT `FK_ActivityUser_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `activityuser`
--

LOCK TABLES `activityuser` WRITE;
/*!40000 ALTER TABLE `activityuser` DISABLE KEYS */;
INSERT INTO `activityuser` VALUES (1,3),(4,4),(1,9);
/*!40000 ALTER TABLE `activityuser` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `channelmessages`
--

DROP TABLE IF EXISTS `channelmessages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `channelmessages` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ChannelId` int NOT NULL,
  `PostedById` int NOT NULL,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PostedAt` datetime(6) NOT NULL,
  `IsPinned` tinyint(1) NOT NULL,
  `IsVisible` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ChannelMessages_ChannelId` (`ChannelId`),
  KEY `IX_ChannelMessages_PostedById` (`PostedById`),
  CONSTRAINT `FK_ChannelMessages_Channels_ChannelId` FOREIGN KEY (`ChannelId`) REFERENCES `channels` (`ChannelId`) ON DELETE CASCADE,
  CONSTRAINT `FK_ChannelMessages_Users_PostedById` FOREIGN KEY (`PostedById`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `channelmessages`
--

LOCK TABLES `channelmessages` WRITE;
/*!40000 ALTER TABLE `channelmessages` DISABLE KEYS */;
INSERT INTO `channelmessages` VALUES (1,1,2,'1','1','2025-07-29 06:57:33.491000',1,1),(2,2,2,'string','string','2025-08-05 05:55:55.326915',1,1),(3,2,2,'string','string','2025-08-05 06:18:05.008629',1,1),(4,1,2,'123','123123213','2025-08-05 06:18:22.892933',1,1),(5,5,2,'t','t','2025-08-06 07:28:30.840086',1,1);
/*!40000 ALTER TABLE `channelmessages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `channelreports`
--

DROP TABLE IF EXISTS `channelreports`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `channelreports` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ChannelId` int NOT NULL,
  `ReportedById` int NOT NULL,
  `Reason` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Status` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ReportedAt` datetime(6) NOT NULL,
  `ReviewedAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ChannelReports_ChannelId` (`ChannelId`),
  KEY `IX_ChannelReports_ReportedById` (`ReportedById`),
  CONSTRAINT `FK_ChannelReports_Channels_ChannelId` FOREIGN KEY (`ChannelId`) REFERENCES `channels` (`ChannelId`) ON DELETE CASCADE,
  CONSTRAINT `FK_ChannelReports_Users_ReportedById` FOREIGN KEY (`ReportedById`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `channelreports`
--

LOCK TABLES `channelreports` WRITE;
/*!40000 ALTER TABLE `channelreports` DISABLE KEYS */;
INSERT INTO `channelreports` VALUES (1,1,3,'11','approved','2025-07-29 06:57:59.582422','2025-08-06 05:03:32.704418'),(2,2,6,'啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊','approved','2025-08-04 08:23:42.849342','2025-08-06 07:01:39.812052'),(3,3,1,'有大问题','approved','2025-08-06 07:08:47.961237','2025-08-06 07:23:56.316988');
/*!40000 ALTER TABLE `channelreports` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `channelrequest`
--

DROP TABLE IF EXISTS `channelrequest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `channelrequest` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `OrganizerId` int NOT NULL,
  `UserId` int NOT NULL,
  `ChannelId` int NOT NULL,
  `Status` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RequestedAt` datetime(6) NOT NULL,
  `ReviewedAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_channelRequest_ChannelId` (`ChannelId`),
  KEY `IX_channelRequest_UserId` (`UserId`),
  CONSTRAINT `FK_channelRequest_Channels_ChannelId` FOREIGN KEY (`ChannelId`) REFERENCES `channels` (`ChannelId`) ON DELETE CASCADE,
  CONSTRAINT `FK_channelRequest_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `channelrequest`
--

LOCK TABLES `channelrequest` WRITE;
/*!40000 ALTER TABLE `channelrequest` DISABLE KEYS */;
INSERT INTO `channelrequest` VALUES (1,2,2,1,'approved','2025-07-29 06:57:04.993939','2025-07-29 06:57:15.569021'),(2,2,2,2,'approved','2025-08-01 07:21:33.604912','2025-08-06 07:00:22.582326'),(3,6,6,3,'approved','2025-08-04 08:23:12.896053','2025-08-06 07:07:39.281650'),(4,6,6,4,'pending','2025-08-05 04:51:58.900494',NULL),(5,2,2,5,'approved','2025-08-05 04:55:50.644117','2025-08-06 07:24:02.481621'),(6,2,2,1,'approved','2025-08-05 05:09:39.428093','2025-08-06 07:15:16.620237'),(7,2,2,1,'approved','2025-08-05 05:40:30.433930','2025-08-06 07:46:55.286323'),(8,2,2,5,'pending','2025-08-05 05:49:16.337787',NULL),(9,2,2,5,'approved','2025-08-05 05:49:24.383060','2025-08-07 03:27:17.943378'),(10,2,2,2,'rejected','2025-08-05 06:02:27.491354','2025-08-07 03:41:17.955873'),(11,2,2,1,'approved','2025-08-06 05:29:33.359209','2025-08-06 07:46:51.780548'),(12,2,2,6,'pending','2025-08-06 07:28:22.790031',NULL),(13,2,2,7,'pending','2025-08-07 03:39:04.708350',NULL);
/*!40000 ALTER TABLE `channelrequest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `channels`
--

DROP TABLE IF EXISTS `channels`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `channels` (
  `ChannelId` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedBy` int NOT NULL,
  `status` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `url` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ChannelId`),
  KEY `IX_Channels_CreatedBy` (`CreatedBy`),
  CONSTRAINT `FK_Channels_Users_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `channels`
--

LOCK TABLES `channels` WRITE;
/*!40000 ALTER TABLE `channels` DISABLE KEYS */;
INSERT INTO `channels` VALUES (1,'1111',2,'approved','str111ing111','111'),(2,'jbfjdaib',2,'rejected','asinfoasf','aaaaaaaaaaaa'),(3,'1234',6,'approved','string','string'),(4,'channel4',6,'pending','channel4','channel4'),(5,'newchannel',2,'approved','newchannel','newchannel'),(6,'t',2,'pending','t','t'),(7,'History',2,'pending','History','History');
/*!40000 ALTER TABLE `channels` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `channeltag`
--

DROP TABLE IF EXISTS `channeltag`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `channeltag` (
  `ChannelId` int NOT NULL,
  `TagId` int NOT NULL,
  PRIMARY KEY (`ChannelId`,`TagId`),
  KEY `IX_ChannelTag_TagId` (`TagId`),
  CONSTRAINT `FK_ChannelTag_Channels_ChannelId` FOREIGN KEY (`ChannelId`) REFERENCES `channels` (`ChannelId`) ON DELETE CASCADE,
  CONSTRAINT `FK_ChannelTag_Tags_TagId` FOREIGN KEY (`TagId`) REFERENCES `tags` (`TagId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `channeltag`
--

LOCK TABLES `channeltag` WRITE;
/*!40000 ALTER TABLE `channeltag` DISABLE KEYS */;
INSERT INTO `channeltag` VALUES (1,10),(2,11),(3,12),(4,13),(5,14),(6,15),(7,58);
/*!40000 ALTER TABLE `channeltag` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `channeluser`
--

DROP TABLE IF EXISTS `channeluser`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `channeluser` (
  `ChannelsChannelId` int NOT NULL,
  `MembersUserId` int NOT NULL,
  PRIMARY KEY (`ChannelsChannelId`,`MembersUserId`),
  KEY `IX_ChannelUser_MembersUserId` (`MembersUserId`),
  CONSTRAINT `FK_ChannelUser_Channels_ChannelsChannelId` FOREIGN KEY (`ChannelsChannelId`) REFERENCES `channels` (`ChannelId`) ON DELETE CASCADE,
  CONSTRAINT `FK_ChannelUser_Users_MembersUserId` FOREIGN KEY (`MembersUserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `channeluser`
--

LOCK TABLES `channeluser` WRITE;
/*!40000 ALTER TABLE `channeluser` DISABLE KEYS */;
INSERT INTO `channeluser` VALUES (1,2),(2,2),(5,2),(6,2),(7,2),(1,3),(3,6),(4,6);
/*!40000 ALTER TABLE `channeluser` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `systemmessages`
--

DROP TABLE IF EXISTS `systemmessages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `systemmessages` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ReceiverId` int NOT NULL,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsRead` tinyint(1) NOT NULL,
  `SentAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SystemMessages_ReceiverId` (`ReceiverId`),
  CONSTRAINT `FK_SystemMessages_Users_ReceiverId` FOREIGN KEY (`ReceiverId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `systemmessages`
--

LOCK TABLES `systemmessages` WRITE;
/*!40000 ALTER TABLE `systemmessages` DISABLE KEYS */;
INSERT INTO `systemmessages` VALUES (1,1,'string','string',1,'2025-08-04 05:56:13.930477'),(2,2,'新用户申请活动','6 申请注册活动：2',1,'2025-08-04 08:11:00.204115'),(3,1,'新活动申请','6 创建了新活动：8 描述为： 8',1,'2025-08-04 08:12:00.663883'),(4,1,'新channel申请','5 创建了新cahnnel：1234 描述为： string',1,'2025-08-04 08:23:12.904313'),(5,1,'新channel举报','5 举报了cahnnel：jbfjdaib 原因为： 有问题',0,'2025-08-04 08:23:42.850106'),(6,1,'新活动申请','5 创建了新活动：1 描述为： string',1,'2025-08-04 08:31:59.438381'),(7,1,'新活动申请','5 创建了新活动：zxc 描述为： zxc',0,'2025-08-04 08:57:08.155001'),(8,1,'新活动申请','1 创建了新活动：123 描述为： 123',0,'2025-08-04 09:09:42.880311'),(9,1,'活动信息修改','1 创建了新活动：1 描述为： 1',0,'2025-08-04 09:28:16.677765'),(10,1,'活动信息修改','1 创建了新活动：1 描述为： 1',0,'2025-08-04 09:44:07.260918'),(11,1,'活动信息修改','5 创建了新活动：zxc 描述为： zxccccc',0,'2025-08-04 09:44:58.056816'),(12,1,'活动信息修改','1 创建了新活动：1 描述为： 1',0,'2025-08-04 11:40:37.469198'),(13,1,'新channel申请','5 创建了新cahnnel：channel4 描述为： channel4',0,'2025-08-05 04:51:58.903835'),(14,1,'新channel申请','1 创建了新cahnnel：newchannel 描述为： newchannel',0,'2025-08-05 04:55:50.645311'),(15,1,'频道更新申请','1 更新了频道：1111 描述为： 1111',0,'2025-08-05 05:09:39.428816'),(16,1,'频道更新申请','1 更新了频道：1111 描述为： str111ing',0,'2025-08-05 05:40:30.434539'),(17,1,'频道更新申请','1 更新了频道：newchannel 描述为： newchannel',0,'2025-08-05 05:49:16.338292'),(18,1,'频道更新申请','1 更新了频道：newchannel 描述为： newchannel',0,'2025-08-05 05:49:24.383420'),(19,1,'频道更新申请','1 更新了频道：jbfjdaib 描述为： asinfoasf',0,'2025-08-05 06:02:27.493982'),(20,1,'活动信息修改','1 创建了新活动：1 描述为： 1',0,'2025-08-05 10:18:30.998689'),(21,2,'新用户申请活动','4 申请注册活动：1',1,'2025-08-05 10:20:19.090915'),(22,2,'新用户申请活动','4 申请注册活动：4',1,'2025-08-06 03:00:48.520154'),(23,2,'新用户申请活动','3 申请注册活动：4',1,'2025-08-06 03:01:18.745020'),(24,1,'活动信息修改','1 创建了新活动：1 描述为： 1111',0,'2025-08-06 05:29:15.064512'),(25,1,'频道更新申请','1 更新了频道：1111 描述为： str111ing111',0,'2025-08-06 05:29:33.360311'),(26,1,'新channel举报','0 举报了cahnnel：1234 原因为： 有大问题',0,'2025-08-06 07:08:47.966555'),(27,1,'活动信息修改','1 创建了新活动：2 描述为： 22222222',0,'2025-08-06 07:20:06.732271'),(28,1,'活动信息修改','1 创建了新活动：2 描述为： 2222222232222',0,'2025-08-06 07:22:31.957675'),(29,1,'新活动申请','1 创建了新活动：t 描述为： t',0,'2025-08-06 07:27:38.862866'),(30,1,'活动信息修改','1 创建了新活动：5 描述为： 555',0,'2025-08-06 07:28:06.695493'),(31,1,'新channel申请','1 创建了新cahnnel：t 描述为： t',0,'2025-08-06 07:28:22.790644'),(32,5,'注册申请取消','4 已取消对活动 4 的注册申请。',0,'2025-08-06 07:38:42.008868'),(33,5,'注册申请取消','4 已取消对活动 1 的注册申请。',0,'2025-08-06 07:39:42.275073'),(34,2,'新用户申请活动','4 申请注册活动：1',0,'2025-08-06 07:41:38.489427'),(35,5,'注册申请取消','4 已取消对活动 1 的注册申请。',0,'2025-08-06 07:42:08.494553'),(36,1,'新活动申请','1 创建了新活动：13 描述为： 13',0,'2025-08-07 01:47:18.301352'),(37,1,'活动信息修改','1 创建了新活动：1 描述为： 1111',0,'2025-08-07 03:37:20.069447'),(38,1,'新channel申请','1 创建了新cahnnel：History 描述为： History',0,'2025-08-07 03:39:04.708924');
/*!40000 ALTER TABLE `systemmessages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tags`
--

DROP TABLE IF EXISTS `tags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tags` (
  `TagId` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`TagId`)
) ENGINE=InnoDB AUTO_INCREMENT=112 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tags`
--

LOCK TABLES `tags` WRITE;
/*!40000 ALTER TABLE `tags` DISABLE KEYS */;
INSERT INTO `tags` VALUES (10,'Academic','1'),(11,'Anthropology','1'),(12,'ArtExhibition','1'),(13,'ArtTherapy','1'),(14,'Arts','1'),(15,'Astronomy','1'),(16,'AudioProduction','1'),(17,'Biology','1'),(18,'Biotechnology','1'),(19,'Business','1'),(20,'Career','1'),(21,'Certification','1'),(22,'Chemistry','1'),(23,'Coding','1'),(24,'Communication','1'),(25,'Community','1'),(26,'Competition','1'),(27,'Cooking','1'),(28,'Counseling','1'),(29,'Craft','1'),(30,'Creative','1'),(31,'CreativeWriting','1'),(32,'CriminalJustice','1'),(33,'Cultural','1'),(34,'CulturalFestival','1'),(35,'Dance','1'),(36,'DataScience','1'),(37,'Debate','1'),(38,'Design','1'),(39,'EcoFriendly','1'),(40,'Economics','1'),(41,'Engineering','1'),(42,'Entrepreneurship','1'),(43,'EnvironmentalScience','1'),(44,'Ethics','1'),(45,'Experimental','1'),(46,'Fashion','1'),(47,'FilmScreening','1'),(48,'Finance','1'),(49,'Fitness','1'),(50,'FoodTasting','1'),(51,'GameDesign','1'),(52,'GameNight','1'),(53,'Gardening','1'),(54,'Geology','1'),(55,'Government','1'),(56,'HandsOn','1'),(57,'Health','1'),(58,'History','1'),(59,'Illustration','1'),(60,'Inclusion','1'),(61,'Innovation','1'),(62,'Interactive','1'),(63,'International','1'),(64,'Investment','1'),(65,'Journalism','1'),(66,'LanguageExchange','1'),(67,'Law','1'),(68,'Leadership','1'),(69,'Marketing','1'),(70,'Media','1'),(71,'Meditation','1'),(72,'Mindfulness','1'),(73,'MobileApp','1'),(74,'Music','1'),(75,'Nature','1'),(76,'Networking','1'),(77,'Neuroscience','1'),(78,'Performance','1'),(79,'PersonalDevelopment','1'),(80,'Philosophy','1'),(81,'Photography','1'),(82,'Physics','1'),(83,'Productivity','1'),(84,'Psychology','1'),(85,'Reading','1'),(86,'Recreation','1'),(87,'ResearchSymposium','1'),(88,'ResumeWorkshop','1'),(89,'Safety','1'),(90,'Science','1'),(91,'Security','1'),(92,'Social','1'),(93,'SocialJustice','1'),(94,'Sports','1'),(95,'StressRelief','1'),(96,'Sustainability','1'),(97,'TableTennis','1'),(98,'Teamwork','1'),(99,'Tech','1'),(100,'TechDemo','1'),(101,'Theater','1'),(102,'Therapy','1'),(103,'Thrifting','1'),(104,'VirtualReality','1'),(105,'Wellness','1'),(106,'Wildlife','1'),(107,'Workshop','1'),(108,'Writing','1'),(109,'Yoga','1'),(110,'Ademic','1');
/*!40000 ALTER TABLE `tags` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userfavouriteactivity`
--

DROP TABLE IF EXISTS `userfavouriteactivity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userfavouriteactivity` (
  `ActivityId` int NOT NULL,
  `UserId` int NOT NULL,
  PRIMARY KEY (`ActivityId`,`UserId`),
  KEY `IX_UserFavouriteActivity_UserId` (`UserId`),
  CONSTRAINT `FK_UserFavouriteActivity_Activities_ActivityId` FOREIGN KEY (`ActivityId`) REFERENCES `activities` (`ActivityId`) ON DELETE CASCADE,
  CONSTRAINT `FK_UserFavouriteActivity_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userfavouriteactivity`
--

LOCK TABLES `userfavouriteactivity` WRITE;
/*!40000 ALTER TABLE `userfavouriteactivity` DISABLE KEYS */;
INSERT INTO `userfavouriteactivity` VALUES (1,3),(2,3),(3,3),(9,10),(11,10),(12,10),(20,10),(21,10),(34,10),(38,10),(43,10),(98,10),(108,10),(119,10),(122,10),(7,11),(23,11),(32,11),(42,11),(43,11),(106,11),(119,11),(122,11),(1,12),(7,12),(9,12),(11,12),(21,12),(23,12),(25,12),(34,12),(43,12),(49,12),(106,12),(119,12),(122,12),(130,12),(135,12),(12,13),(17,13),(34,13),(57,13),(96,13),(122,13),(6,14),(9,14),(17,14),(32,14),(49,14),(96,14),(108,14),(120,14),(127,14),(130,14),(147,14),(1,15),(7,15),(24,15),(28,15),(51,15),(55,15),(60,15),(67,15),(77,15),(87,15),(93,15),(109,15),(121,15),(133,15),(4,16),(8,16),(29,16),(54,16),(57,16),(64,16),(114,16),(3,17),(10,17),(15,17),(26,17),(48,17),(53,17),(104,17),(126,17),(136,17),(138,17),(2,18),(20,18),(37,18),(64,18),(120,18),(127,18),(131,18),(65,19),(73,19),(75,19),(88,19),(95,19),(7,20),(16,20),(27,20),(31,20),(109,20),(115,20),(5,21),(19,21),(22,21),(52,21),(61,21),(76,21),(81,21),(85,21),(97,21),(110,21),(118,21),(141,21),(9,22),(23,22),(25,22),(43,22),(49,22),(86,22),(96,22),(98,22),(120,22),(122,22),(138,22),(144,22),(146,22),(6,23),(32,23),(47,23),(82,23),(108,23),(147,23),(15,24),(35,24),(45,24),(51,24),(65,24),(68,24),(78,24),(99,24),(112,24),(115,24),(123,24),(139,24),(143,24),(46,25),(64,25),(87,25),(95,25),(107,25),(149,25),(4,26),(11,26),(46,26),(49,26),(57,26),(66,26),(90,26),(92,26),(103,26),(114,26),(124,26),(141,26),(21,27),(73,27),(91,27),(113,27),(131,27),(6,28),(29,28),(32,28),(47,28),(134,28),(21,29),(37,29),(56,29),(66,29),(142,29);
/*!40000 ALTER TABLE `userfavouriteactivity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userprofiles`
--

DROP TABLE IF EXISTS `userprofiles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userprofiles` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `Age` int DEFAULT NULL,
  `Gender` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `url` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_UserProfiles_UserId` (`UserId`),
  CONSTRAINT `FK_UserProfiles_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userprofiles`
--

LOCK TABLES `userprofiles` WRITE;
/*!40000 ALTER TABLE `userprofiles` DISABLE KEYS */;
INSERT INTO `userprofiles` VALUES (1,2,111,'stri111ng','33333'),(2,3,10,'string','2222'),(3,10,25,'Male','https://example.com/profile/9094'),(4,11,35,'Male','https://example.com/profile/8921'),(5,12,39,'Male','https://example.com/profile/8189'),(6,13,29,'Female','https://example.com/profile/2043'),(7,14,30,'Male','https://example.com/profile/4078'),(8,15,36,'Female','https://example.com/profile/9208'),(9,16,28,'Male','https://example.com/profile/1621'),(10,17,19,'Male','https://example.com/profile/6422'),(11,18,40,'Female','https://example.com/profile/4820'),(12,19,38,'Female','https://example.com/profile/6433'),(13,20,40,'Male','https://example.com/profile/7651'),(14,21,37,'Male','https://example.com/profile/7485'),(15,22,33,'Female','https://example.com/profile/9133'),(16,23,34,'Male','https://example.com/profile/7520'),(17,24,34,'Male','https://example.com/profile/4690'),(18,25,33,'Male','https://example.com/profile/5205'),(19,26,39,'Female','https://example.com/profile/7911'),(20,27,33,'Female','https://example.com/profile/4046'),(21,28,26,'Female','https://example.com/profile/4370'),(22,29,28,'Male','https://example.com/profile/6010');
/*!40000 ALTER TABLE `userprofiles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userprofiletag`
--

DROP TABLE IF EXISTS `userprofiletag`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userprofiletag` (
  `TagId` int NOT NULL,
  `UserProfileId` int NOT NULL,
  PRIMARY KEY (`TagId`,`UserProfileId`),
  KEY `IX_UserProfileTag_UserProfileId` (`UserProfileId`),
  CONSTRAINT `FK_UserProfileTag_Tags_TagId` FOREIGN KEY (`TagId`) REFERENCES `tags` (`TagId`) ON DELETE CASCADE,
  CONSTRAINT `FK_UserProfileTag_UserProfiles_UserProfileId` FOREIGN KEY (`UserProfileId`) REFERENCES `userprofiles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userprofiletag`
--

LOCK TABLES `userprofiletag` WRITE;
/*!40000 ALTER TABLE `userprofiletag` DISABLE KEYS */;
INSERT INTO `userprofiletag` VALUES (10,1),(10,2),(14,3),(86,3),(92,3),(14,4),(92,4),(100,4),(14,5),(92,5),(100,5),(14,6),(86,6),(92,6),(14,7),(86,7),(92,7),(23,8),(36,8),(99,8),(71,9),(75,9),(105,9),(20,10),(42,10),(26,11),(49,11),(94,11),(10,12),(87,12),(90,12),(12,13),(30,13),(38,13),(39,14),(43,14),(96,14),(14,15),(74,15),(78,15),(33,16),(63,16),(66,16),(29,17),(56,17),(107,17),(57,18),(105,18),(75,19),(81,19),(37,20),(55,20),(27,21),(50,21),(63,21);
/*!40000 ALTER TABLE `userprofiletag` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserId` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Role` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `status` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'0','0@0.com','0','admin','active'),(2,'1','01@0.com','1','organizer','active'),(3,'2','012@0.com','2','user','active'),(4,'3','3@3.com','123','user','active'),(5,'4','4@4.com','4','user','active'),(6,'5','5@5.com','5','organizer','active'),(9,'6','6@6.com','6','organizer','active'),(10,'JwEysTYw','ezvxlw@test.org','581713','user','active'),(11,'ojJINQEN','rxrtgy@example.com','696844','user','active'),(12,'TcRtWClq','krjgsl@example.com','121818','user','active'),(13,'faPFSKSa','apytwb@test.org','617003','user','active'),(14,'kCWSqHOc','uivuwf@example.com','873448','user','active'),(15,'pWdPBPtD','uaoaru@example.com','918677','user','active'),(16,'nqSoUCDb','vhdibb@mail.com','101864','user','active'),(17,'oFgVrQCC','xtmdkp@mail.com','213371','user','active'),(18,'wxseRxSd','zzskey@example.com','117285','user','active'),(19,'lqYkxkOM','rslwag@test.org','200672','user','active'),(20,'gNqDSqqa','wvfuqk@mail.com','770322','user','active'),(21,'DENtebry','inriik@example.com','726561','user','active'),(22,'DLpLinpI','fouvhj@example.com','451729','user','active'),(23,'UyURhMdY','kwztzr@mail.com','862911','user','active'),(24,'DimqplpG','jjxrkk@test.org','940023','user','active'),(25,'RubdrdtE','nblxzc@example.com','139593','user','active'),(26,'aIMKlHik','huompk@mail.com','76429','user','active'),(27,'udKDFMwM','ypyxzd@test.org','490015','user','active'),(28,'acUbTTNw','zwuazp@mail.com','597081','user','active'),(29,'SxXLdySf','avhjyi@test.org','176571','user','active');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-08-07 14:25:07
