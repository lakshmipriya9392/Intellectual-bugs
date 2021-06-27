import React from 'react'
import Navbar from './../navbar/navbar'
import { motion } from 'framer-motion'
import axios from 'axios'

function signIn() {


    const handleSubmit = (e) => {
        e.preventDefault();


        const data = {
            email : 'email',
            password : 'password'

        }
        axios.post('https://localhost:5001/user/login', data)
      .then(res => {
          console.log(res)
      }).catch(err => {
          console.log(err)
      })

    }


    let Validation = () => {
        //The below code is for name validation

        let name = document.querySelector("#name").value
        let nameWarn = document.querySelector("#name-warn")
        let nameCheck = name.trim()
        if (name == "") {
            nameWarn.textContent = "Name cannot be empty"
        }
        else if (nameCheck == "") {
            nameWarn.textContent = "Name cannot contain only spaces"
        }
        else {
            nameWarn.textContent = ""
        }

        //The below code is of password validation

        let password1 = document.querySelector("#password").value
        let password1Length = password1.length
        let passwordWarn = document.querySelector("#password-warn")

        if (password1Length < 8) {
            passwordWarn.textContent = "Your password must be 8 chacracters long"
        }

        else if ((!(password1.includes("1"))) &&
            (!(password1.includes("2"))) &&
            (!(password1.includes("3"))) &&
            (!(password1.includes("4"))) &&
            (!(password1.includes("5"))) &&
            (!(password1.includes("6"))) &&
            (!(password1.includes("7"))) &&
            (!(password1.includes("8"))) &&
            (!(password1.includes("9"))) &&
            (!(password1.includes("0")))
        ) {
            passwordWarn.textContent = "Your password must contain numbers"
        }

        else {
            passwordWarn.textContent = ""
        }

        if (passwordWarn.textContent == "" && nameWarn.textContent == "") {
            console.log('Proceed')
        }


    }

    let Clear = () => {
        document.querySelector("#name").value = ""
        document.querySelector("#password").value = ""

        let nameWarn = document.querySelector("#name-warn")
        let passwordWarn = document.querySelector("#password-warn")
        nameWarn.textContent = ""
        passwordWarn.textContent = ""
    }

    return (
        <div
            className='w-screen h-screen overflow-hidden'>
            <motion.div
                initial={{ marginTop: '-100vh' }}
                animate={{ marginTop: 0 }}
                transition={{ delay: 0.5, type: 'spring', stiffness: 250 }}
            >
                <Navbar />
                <div className="flex justify-center">

                    <form action="" className="text-sm shadow-2xl w-11/12 md:w-5/12 h-auto mt-16 mb-2 pb-5 rounded-2xl border-4 flex justify-center flex-col items-center">
                        <div className="text-2xl my-5">Sign In</div>

                        <input type="email" name="" id="" className="my-4 w-3/4 outline-none border-b-2" placeholder="Email" id="name" />
                        <div className="text-red-500 text-xs" id="name-warn"></div>

                        <input type="password" name="" id="" className="my-4 w-3/4 outline-none border-b-2" placeholder="Your Password" id="password" />
                        <div className="text-red-500 text-xs" id="password-warn"></div>

                        <div className="flex justify-between w-full px-16 pt-6 pb-4">
                            <button type="button" className="text-xs md:text-sm border-2 border-black outline-none px-2 py-1 rounded-lg"
                                onClick={Clear}>Clear All</button>
                            <button type="button" className="text-xs md:text-sm border-2 border-black outline-none px-2 py-1 rounded-lg"
                                onClick={handleSubmit}
                            >Submit</button>
                        </div>
                    </form>
                </div>
            </motion.div>
        </div>
    )
}

export default signIn